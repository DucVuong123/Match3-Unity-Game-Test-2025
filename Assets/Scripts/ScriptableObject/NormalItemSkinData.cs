using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minigame.Mathch3
{
    [CreateAssetMenu(fileName = "NormalItemSkinData", menuName = "Game/Normal Item Skin Data")]
    public class NormalItemSkinData : ScriptableObject
    {
        [Serializable]
        public class SkinEntry
        {
            public NormalItem.eNormalType Type;
            public Sprite Sprite;
        }

        public List<SkinEntry> Skins = new List<SkinEntry>();

        public Sprite GetSprite(NormalItem.eNormalType type)
        {
            for (int i = 0; i < Skins.Count; i++)
            {
                if (Skins[i].Type == type)
                {
                    return Skins[i].Sprite;
                }
            }

            return null;
        }
    }
}
