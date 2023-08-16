using UnityEngine;
using UnityEditor;

namespace Meangpu.Gacha
{
    [CustomPropertyDrawer(typeof(GachaWithRate))]
    public class GachaWithRateDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            SerializedProperty Object = property.FindPropertyRelative("Object");
            SerializedProperty SpawnRate = property.FindPropertyRelative("Rate");

            const int column = 3;
            const int boolTickSpaceRation = 1;
            const int widthRatio = column - boolTickSpaceRation;

            float widthSize = position.width / column;

            const float offsetSize = 5;

            Rect pos1 = new(position.x, position.y, (widthSize * widthRatio) - offsetSize, position.height);
            Rect pos2 = new(position.x + (widthSize * widthRatio), position.y, widthSize - offsetSize, position.height);

            EditorGUI.PropertyField(pos1, Object, GUIContent.none);
            EditorGUI.PropertyField(pos2, SpawnRate, GUIContent.none);

            EditorGUI.EndProperty();
        }
    }
}