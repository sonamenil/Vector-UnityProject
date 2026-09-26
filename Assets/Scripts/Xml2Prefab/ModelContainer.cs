using Nekki.Vector.Core.User;
using Nekki.Vector.Core.Utilites;
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Xml2Prefab
{
    [Serializable]
    public class ModelsContainer
    {
        public ChoiceContainer Choice;

        public List<ModelContainer> Models;

        public void Init(XmlNode node)
        {
            if (node == null)
            {
                return;
            }

            Choice = new ChoiceContainer(node.Attributes["Choice"].ParseString(), node.Attributes["Variant"].ParseString());

            foreach (XmlNode child in node.ChildNodes)
            {
                var model = new ModelContainer();
                model.Init(child);

                Models.Add(model);
            }
        }
    }

    [Serializable]
    public class ModelContainer
    {
        public string Name = "Player";

        public string BirthSpawn = "DefaultSpawn";

        public bool IsPlayer = true;

        public string[] Skins;

        public string[] Stocks;

        public string[] Arrests;

        public string[] Murders;

        public string[] Respawns;

        public string[] AllowedSpawns;

        public Color Color = Color.black;

        public int AI = 0;

        public float SpawnTime;

        public float LifeTime = 2;

        public bool IsTrick = true;

        public bool IsItem = true;

        public bool IsVictory = true;

        public bool IsLost = true;

        public bool IsIcon;

        public void Init(XmlNode node)
        {
            Name = node.Attributes["Name"].Value;
            BirthSpawn = node.Attributes["BirthSpawn"].ParseString("");
            IsIcon = node.Attributes["Icon"].ParseBool();
            Color = node.Attributes["Color"] == null ? Color.black : ColorUtils.FromHex(node.Attributes["Color"].Value);
            Skins = node.Attributes["Skins"] == null ? null : node.Attributes["Skins"].Value.Split('|');
            Stocks = node.Attributes["Stocks"] == null ? null : node.Attributes["Stocks"].Value.Split('|');
            Arrests = node.Attributes["Arrests"] == null ? null : node.Attributes["Arrests"].Value.Split('|');
            Murders = node.Attributes["Murders"] == null ? null : node.Attributes["Murders"].Value.Split('|');
            Respawns = node.Attributes["Respawns"] == null ? null : node.Attributes["Respawns"].Value.Split('|');
            AllowedSpawns = node.Attributes["AllowedSpawns"] == null ? null : node.Attributes["AllowedSpawns"].Value.Split('|');
            IsPlayer = node.Attributes["Type"].ParseBool();
            IsTrick = node.Attributes["Trick"].ParseBool();
            IsItem = node.Attributes["Item"].ParseBool();
            IsVictory = node.Attributes["Victory"].ParseBool();
            IsLost = node.Attributes["Lose"].ParseBool();
            AI = int.Parse(node.Attributes["AI"].Value);
            SpawnTime = node.Attributes["Time"].ParseFloat();
            LifeTime = node.Attributes["LifeTime"].ParseFloat(2);
        }
    }
}

