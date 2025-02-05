using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Character Type", fileName = "New Character Type")]
public class CharacterTypeSO : ScriptableObject
{
    [field: SerializeField] public string CharacterTypeName { get; private set; }

    [field: SerializeField] public Sprite CharacterPreviewSpritesheet { get; private set; }
    [field: SerializeField] public RuntimeAnimatorController CharacterController { get; private set; }
    [field: SerializeField] public float PlayerCharacterSpeed { get; private set; } = 6.5f;

    [field: Space]

    [field: SerializeField] public Vector2 SpriteSize_16x16 { get; private set; }
    [field: SerializeField] public Vector2 SpriteSize_32x32 { get; private set; }
    [field: SerializeField] public Vector2 SpriteSize_48x48 { get; private set; }

    [field: Space]

    [field: SerializeField] public CharacterPieceCollection[] CharacterPieces { get; private set; }

    [field: Space]

    [field: SerializeField] public List<CharacterBackup> CharacterModificationHistory { get; set; }
    [field: SerializeField] public List<CharacterBackup> CharacterSaveHistory { get; set; }

    public void Init()
    {
        ClearSprites();
        CharacterModificationHistory.Clear();
        CharacterSaveHistory.Clear();
        foreach (CharacterPieceCollection characterPiece in CharacterPieces)
        {
            characterPiece.DropdownIndex = 0;
            characterPiece.CanRandomize = PlayerPrefs.GetInt(characterPiece.spriteLocation, 1) == 1;
        }
    }

    public void ClearSprites()
    {
        foreach (CharacterPieceCollection characterPiece in CharacterPieces)
        {
            characterPiece.Sprites.Clear();
        }
    }

    public void SaveRandomizeToggles()
    {
        foreach (CharacterPieceCollection characterPiece in CharacterPieces)
        {
            PlayerPrefs.SetInt(characterPiece.spriteLocation, characterPiece.CanRandomize ? 1 : 0);
        }
    }

    [System.Serializable]
    public class CharacterPieceCollection
    {
        public string CollectionName;

        [Tooltip("The location on the users computer where the sprites are located")]
        public string spriteLocation;

        [field: Space]

        [field: SerializeField] public bool IncludeNAOption { get; private set; } = false;
        [field: SerializeField, ShowIf("IncludeNAOption")] public bool NADefault { get; private set; } = true;

        [Space]

        [ReadOnly] public List<Sprite> Sprites;

        [field: SerializeField, ReadOnly] public Sprite ActiveSprite { get; private set; }

        public bool CanRandomize { get; set; } = true;

        public int DropdownIndex { get; set; } = 0;

        public void SetActiveSprite(int index)
        {
            if (Sprites.Count == 0) return;

            if (IncludeNAOption)
            {
                if (index == 0)
                    ActiveSprite = null;
                else
                    ActiveSprite = Sprites[index - 1];
            }
            else
            {
                ActiveSprite = Sprites[index];
            }
        }
    }

    [System.Serializable]
    public class CharacterBackup
    {
        public Sprite CharacterPreviewSprite;

        public int[] CharacterPieceIndexes;

        public CharacterBackup(Sprite characterPreviewSprite, int[] characterPieceIndexes)
        {
            CharacterPreviewSprite = characterPreviewSprite;
            CharacterPieceIndexes = characterPieceIndexes;
        }
    }
}