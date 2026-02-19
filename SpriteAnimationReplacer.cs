using UnityEngine;

[ExecuteInEditMode]
public class SpriteAnimationReplacer : MonoBehaviour
{
	[Header("Assign the clip to modify")]
	public AnimationClip targetClip;

	[Header("Assign the NEW sprite sheet (PNG, already sliced)")]
	public Texture2D newSpriteSheet;

	[Header("Options")]
	[Tooltip("If ON: try to match keyframe sprite by name (or trailing index).\nIf OFF: map by keyframe index -> sprite index.")]
	public bool matchByName;

	[Tooltip("If more keyframes than sprites: loop sprites.")]
	public bool wrapWhenOutOfRange = true;

	[Tooltip("Sort sprites left-to-right (and top-to-bottom) by their rect position.\nTurn OFF if you prefer name/index sorting.")]
	public bool sortBySheetPosition = true;
}
