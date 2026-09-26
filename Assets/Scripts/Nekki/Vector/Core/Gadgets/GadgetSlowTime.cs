using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI;
using UnityEngine;

namespace Nekki.Vector.Core.Gadgets
{
    public class GadgetSlowTime : Gadget
    {
        public const float Duration = 3f;

        public const float Cooldown = 5f;

        public const float SlowModeRampDuration = 1;

        public float CurrentCooldown = 0;

        public GadgetSlowTime() : base(GadgetType.KillBot)
        {
        }

        public override void Play()
        {
            base.Play();
            CoroutineRunner.Instance.Run(SlowTime());
            CurrentCooldown = Cooldown;
        }

        public override void Stop()
        {
            base.Stop();
            CoroutineRunner.Instance.Run(CooldownTime());
        }

        public IEnumerator SlowTime()
        {
            float time = Duration;
            float rampTime = 0f;

            float startSlowModeFrames = LevelMainController.current != null
                ? LevelMainController.current.slowModeFrames
                : 1;

            while (time > 0 && LevelMainController.current != null)
            {
                if (!LevelMainController.current.pauseRender)
                {
                    if (rampTime < SlowModeRampDuration)
                    {
                        rampTime += Time.deltaTime;

                        float rampProgress = Mathf.Clamp01(rampTime / SlowModeRampDuration);
                        LevelMainController.current.slowModeFrames =
                            Mathf.Lerp(startSlowModeFrames, 0.1f, rampProgress);
                    }
                    else
                    {
                        LevelMainController.current.slowModeFrames = 0.1f;
                    }

                    time -= Time.deltaTime;

                    float progress = Mathf.Clamp01(1f - (time / Duration));
                    GameplayView.Current.GadgetCooldownIcon.fillAmount = progress;
                }

                yield return null;
            }

            if (LevelMainController.current != null)
            {
                LevelMainController.current.slowModeFrames = 1;
            }

            Stop();
        }

        public IEnumerator CooldownTime()
        {
            while (CurrentCooldown > 0 && LevelMainController.current != null)
            {
                if (!LevelMainController.current.pauseRender)
                {
                    CurrentCooldown -= Time.deltaTime;

                    float progress = Mathf.Clamp01(CurrentCooldown / Cooldown);
                    GameplayView.Current.GadgetCooldownIcon.fillAmount = progress;
                }
                yield return null;
            }

            CurrentCooldown = 0;
            yield break;
        }

        public override bool IsCanUse()
        {
            return CurrentCooldown <= 0 && !LevelMainController.current.tutorialPause;
        }

    }
}
