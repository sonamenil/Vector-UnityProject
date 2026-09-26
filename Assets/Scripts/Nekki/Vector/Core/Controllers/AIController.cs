using Nekki.Vector.Core.Controllers;
using Nekki.Vector.Core.Detector;
using Nekki.Vector.Core.Models;
using Nekki.Vector.Core.Node;
using UnityEngine;

public class AIController : MonoBehaviour
{
    const string RunFastAreaName = "TriggerRunFast";
    const string WallJumpAreaName = "TriggerWallJump";

    ModelHuman _Model;

    DetectorLine _DetectorH;
    DetectorLine _DetectorV;
    ModelNode _COM;

    [Header("Ground")]
    [SerializeField] float groundProbeDistance = 60;
    [SerializeField] Vector2 groundProbeOffset = new Vector2(100, -20);

    [SerializeField] float extraGroundProbeDistance = 900;
    [SerializeField] Vector2 extraGroundProbeOffset = new Vector2(420, 0);

    [Header("Gap")]
    [SerializeField] float gapProbeDistance = 850f;
    [SerializeField] Vector2 gapProbeOffset = new Vector2(0, 100);

    [Header("Forward")]
    [SerializeField] float forwardProbeDistance = 410;
    [SerializeField] Vector2 forwardProbeOffset = new Vector2(10, -20);

    [SerializeField] float runFastProbeDistance = 900;

    [Header("Obstacle")]
    [SerializeField] float maxObstacleHeight = 90;
    [SerializeField] float obstacleJumpDistance = 200;
    [SerializeField] float obstacleProbeDistance = 300;
    [SerializeField] Vector2 obstacleProbeOffset = new Vector2(310, 0);

    [Header("Down")]
    [SerializeField] float downProbeDistance = 210;
    [SerializeField] Vector2 downProbeOffset = new Vector2(10, -200);

    [Header("Slide")]
    [SerializeField] float slideProbeDistance = 250;
    [SerializeField] Vector2 slideProbeOffset = new Vector2(-30, -190);

    Collider2D _lastObstacleCollider;
    Collider2D _lastJumpCollider;
    Collider2D _lasSlideCollider;

    Collider2D _lastAreaCollider;

    Key LeftDirection
    {
        get
        {
            return _Model.Sign == 1 ? Key.Left : Key.Right;
        }
    }

    Key RightDirection
    {
        get
        {
            return _Model.Sign == 1 ? Key.Right : Key.Left;
        }
    }

    bool IsAction
    {
        get
        {
            return _Model.IsActionInterval || _Model.AnimationName == "SlopeSlide" || _Model.AnimationName == "SlopeSlideLanding";
        }
    }

    bool IsRunFast
    {
        get
        {
            return _Model.AnimationName.Contains("RunFast");
        }
    }

    public void Init(ModelHuman model)
    {
        _Model = model;

        _DetectorH = model.ModelObject.DetectorHorizontalLine;
        _DetectorV = model.ModelObject.DetectorVerticalLine;
        _COM = model.ModelObject.CenterOfMassNode;
    }

    public void Render()
    {
        if (!IsAction)
        {
            return;
        }

        AreaCheck();

        if (GroundCheck())
        {
            return;
        }

        if (ForwardCheck())
        {
            return;
        }

        if (SlideCheck())
        {
            return;
        }
    }

    bool GroundCheck()
    {
        Vector2 groundPos = ApplyFacingOffset((Vector3)_DetectorH.Start.Start, groundProbeOffset, _Model.Sign);

        var groundHit = Physics2D.Raycast(groundPos, Vector2.up, groundProbeDistance);

        RayDrawer.Instance.DrawRay(new RayDrawer.RayData(groundPos, Vector2.up, 0, groundProbeDistance, groundHit, groundHit));

        if (!groundHit)
        {
            var extraPos = ApplyFacingOffset(groundPos, extraGroundProbeOffset, _Model.Sign);
            var extraGround = Physics2D.Raycast(extraPos, Vector2.up, extraGroundProbeDistance);

            RayDrawer.Instance.DrawRay(new RayDrawer.RayData(extraPos, Vector2.up, 0, extraGroundProbeDistance, extraGround, extraGround));

            var gapPos = ApplyFacingOffset(groundPos, gapProbeOffset, _Model.Sign);

            var gapDirection = _Model.Sign == 1 ? Vector2.right : Vector2.left;

            var gapHit = Physics2D.Raycast(gapPos, gapDirection, gapProbeDistance);

            RayDrawer.Instance.DrawRay(new RayDrawer.RayData(gapPos, gapDirection, 0, gapProbeDistance, gapHit, gapHit));

            if (gapHit && gapHit.fraction > 0.01f)
            {
                Jump(gapHit.collider);
            }
            else if (!extraGround)
            {
                Debug.Log("void jump");
                //just random jump into space
                Jump_Force();
            }

            return true;
        }

        return false;
    }

    bool ForwardCheck()
    {
        Vector2 forwardPos = ApplyFacingOffset((Vector3)_DetectorH.Start.Start, forwardProbeOffset, _Model.Sign);

        var forwardDirection = _Model.Sign == 1 ? Vector2.right : Vector2.left;

        if (IsRunFast)
        {
            var runFastHit = Physics2D.Raycast(forwardPos, forwardDirection, runFastProbeDistance);
            if (runFastHit)
            {
                Side(LeftDirection);
                return true;
            }
        }

        var forwardHit = Physics2D.Raycast(forwardPos, forwardDirection, forwardProbeDistance);

        RayDrawer.Instance.DrawRay(new RayDrawer.RayData(forwardPos, forwardDirection, 0, forwardProbeDistance, forwardHit, forwardHit));

        if (forwardHit && forwardHit.fraction > 0.01f)
        {
            var downPos = ApplyFacingOffset(forwardHit.point, downProbeOffset, _Model.Sign);

            var downHit = Physics2D.Raycast(downPos, Vector2.up, downProbeDistance);

            RayDrawer.Instance.DrawRay(new RayDrawer.RayData(downPos, Vector2.up, 0, downProbeDistance, downHit, downHit));



            var obstaclePos = ApplyFacingOffset(forwardHit.point, obstacleProbeOffset, _Model.Sign);

            var obstacleHit = Physics2D.Raycast(obstaclePos, -forwardDirection, obstacleProbeDistance);

            RayDrawer.Instance.DrawRay(new RayDrawer.RayData(obstaclePos, -forwardDirection, 0, obstacleProbeDistance, obstacleHit, obstacleHit));

            var obstacleGroundHit = Physics2D.Raycast(new Vector2(forwardHit.point.x + forwardDirection.x * 3, (float)_DetectorH.Start.Start.Y + groundProbeOffset.y), Vector2.up, groundProbeDistance);

            if (obstacleGroundHit && obstacleHit && Mathf.Abs(obstacleGroundHit.point.y - downHit.point.y) <= maxObstacleHeight  && obstacleHit.collider != _lastObstacleCollider && obstacleHit.fraction > 0.01f && obstacleHit.collider == forwardHit.collider)
            {
                if (Mathf.Abs((float)_COM.Start.X - forwardHit.point.x) < obstacleJumpDistance)
                {
                    Jump_Force();
                    _lastObstacleCollider = obstacleHit.collider;
                }
                return true;
            }



            if (downHit)
            {
                RayDrawer.Instance.DrawSphere(new RayDrawer.SphereData(downHit.point, 10, Color.yellow));

                Jump(forwardHit.collider);

                return true;
            }
        }

        return false;
    }

    bool SlideCheck()
    {
        var slidePos = ApplyFacingOffset((Vector3)_DetectorH.Start.Start, slideProbeOffset, _Model.Sign);

        var slideDirection = _Model.Sign == 1 ? Vector2.right : Vector2.left;

        var slideHit = Physics2D.Raycast(slidePos, slideDirection, slideProbeDistance);

        RayDrawer.Instance.DrawRay(new RayDrawer.RayData(slidePos, slideDirection, 0, slideProbeDistance, slideHit, slideHit));

        if (slideHit && slideHit.fraction > 0.01f)
        {
            Slide(slideHit.collider);
            return true;
        }
        return false;
    }

    void AreaCheck()
    {
        Vector3 areaPos = _COM.Start;

        Physics2D.queriesHitTriggers = true;

        var areaHit = Physics2D.Raycast(areaPos, Vector2.right, 1);

        Physics2D.queriesHitTriggers = false;


        if (areaHit && areaHit.collider.isTrigger && _lastAreaCollider != areaHit.collider)
        {

            switch (areaHit.collider.name)
            {
                case RunFastAreaName:
                    Side(RightDirection);
                    if (IsRunFast)
                    {
                        _lastAreaCollider = areaHit.collider;
                    }
                    break;
                case WallJumpAreaName:
                    Side(LeftDirection);
                    break;
            }

        }

    }


    public void Reset()
    {
        _lastAreaCollider = null;
        _lastObstacleCollider = null;
        _lastJumpCollider = null;
        _lasSlideCollider = null;
    }

    public static Vector2 ApplyFacingOffset(Vector2 worldPosition, Vector2 localOffset, int sign)
    {
        return worldPosition + new Vector2(localOffset.x * sign, localOffset.y);
    }

    void Jump(Collider2D collider)
    {
        if (_lastJumpCollider != collider)
        {
            _Model.ControllerKeys.SetKeyVariable(new Nekki.Vector.Core.Controllers.KeyVariables(Nekki.Vector.Core.Controllers.Key.Up));
            _lastJumpCollider = collider;
        }
    }

    void Jump_Force()
    {
        _Model.ControllerKeys.SetKeyVariable(new Nekki.Vector.Core.Controllers.KeyVariables(Nekki.Vector.Core.Controllers.Key.Up));
    }

    void Slide(Collider2D collider)
    {
        if (_lasSlideCollider != collider)
        {
            _Model.ControllerKeys.SetKeyVariable(new Nekki.Vector.Core.Controllers.KeyVariables(Nekki.Vector.Core.Controllers.Key.Down));
            _lasSlideCollider = collider;
        }
    }

    void Side(Key side)
    {
        _Model.ControllerKeys.SetKeyVariable(new KeyVariables(side));
    }
}
