// #region Assembly UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// // location unknown
// // Decompiled with ICSharpCode.Decompiler 9.1.0.7988
// #endregion

// using System;
// using System.Collections;
// using UnityEngine.Events;
// using UnityEngine.EventSystems;
// using UnityEngine.Serialization;

// namespace UnityEngine.UI;

// [AddComponentMenu("UI/Button", 30)]
// public class Button : Selectable, IPointerClickHandler, IEventSystemHandler, ISubmitHandler
// {
//     [Serializable]
//     public class ButtonClickedEvent : UnityEvent
//     {
//     }

//     [FormerlySerializedAs("onClick")]
//     [SerializeField]
//     private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

//     public ButtonClickedEvent onClick
//     {
//         get
//         {
//             return m_OnClick;
//         }
//         set
//         {
//             m_OnClick = value;
//         }
//     }

//     protected Button()
//     {
//     }

//     private void Press()
//     {
//         if (IsActive() && IsInteractable())
//         {
//             UISystemProfilerApi.AddMarker("Button.onClick", this);
//             m_OnClick.Invoke();
//         }
//     }

//     public virtual void OnPointerClick(PointerEventData eventData)
//     {
//         if (eventData.button == PointerEventData.InputButton.Left)
//         {
//             Press();
//         }
//     }

//     public virtual void OnSubmit(BaseEventData eventData)
//     {
//         Press();
//         if (IsActive() && IsInteractable())
//         {
//             DoStateTransition(SelectionState.Pressed, instant: false);
//             StartCoroutine(OnFinishSubmit());
//         }
//     }

//     private IEnumerator OnFinishSubmit()
//     {
//         float fadeTime = base.colors.fadeDuration;
//         float elapsedTime = 0f;
//         while (elapsedTime < fadeTime)
//         {
//             elapsedTime += Time.unscaledDeltaTime;
//             yield return null;
//         }

//         DoStateTransition(base.currentSelectionState, instant: false);
//     }
// }
// #if false // Decompilation log
// '264' items in cache
// ------------------
// Resolve: 'netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
// Found single assembly: 'netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
// Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.36f1\Editor\Data\NetStandard\ref\2.1.0\netstandard.dll'
// ------------------
// Resolve: 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
// Found single assembly: 'UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
// Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.36f1\Editor\Data\Managed\UnityEngine\UnityEngine.CoreModule.dll'
// ------------------
// Resolve: 'UnityEngine.UIModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
// Found single assembly: 'UnityEngine.UIModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
// Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.36f1\Editor\Data\Managed\UnityEngine\UnityEngine.UIModule.dll'
// ------------------
// Resolve: 'UnityEngine.TextRenderingModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
// Found single assembly: 'UnityEngine.TextRenderingModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
// Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.36f1\Editor\Data\Managed\UnityEngine\UnityEngine.TextRenderingModule.dll'
// ------------------
// Resolve: 'UnityEngine.PhysicsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
// Found single assembly: 'UnityEngine.PhysicsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
// Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.36f1\Editor\Data\Managed\UnityEngine\UnityEngine.PhysicsModule.dll'
// ------------------
// Resolve: 'UnityEngine.Physics2DModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
// Found single assembly: 'UnityEngine.Physics2DModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
// Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.36f1\Editor\Data\Managed\UnityEngine\UnityEngine.Physics2DModule.dll'
// ------------------
// Resolve: 'UnityEngine.IMGUIModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
// Found single assembly: 'UnityEngine.IMGUIModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
// Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.36f1\Editor\Data\Managed\UnityEngine\UnityEngine.IMGUIModule.dll'
// ------------------
// Resolve: 'UnityEngine.AnimationModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
// Found single assembly: 'UnityEngine.AnimationModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
// Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.36f1\Editor\Data\Managed\UnityEngine\UnityEngine.AnimationModule.dll'
// ------------------
// Resolve: 'UnityEngine.UIElementsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
// Found single assembly: 'UnityEngine.UIElementsModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
// Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.36f1\Editor\Data\Managed\UnityEngine\UnityEngine.UIElementsModule.dll'
// ------------------
// Resolve: 'UnityEngine.InputLegacyModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
// Found single assembly: 'UnityEngine.InputLegacyModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
// Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.36f1\Editor\Data\Managed\UnityEngine\UnityEngine.InputLegacyModule.dll'
// ------------------
// Resolve: 'UnityEditor.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
// Found single assembly: 'UnityEditor.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
// Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.36f1\Editor\Data\Managed\UnityEngine\UnityEditor.CoreModule.dll'
// ------------------
// Resolve: 'UnityEngine.TilemapModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
// Found single assembly: 'UnityEngine.TilemapModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
// Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.36f1\Editor\Data\Managed\UnityEngine\UnityEngine.TilemapModule.dll'
// ------------------
// Resolve: 'UnityEngine.SpriteShapeModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
// Found single assembly: 'UnityEngine.SpriteShapeModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null'
// Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.36f1\Editor\Data\Managed\UnityEngine\UnityEngine.SpriteShapeModule.dll'
// ------------------
// Resolve: 'System.Runtime.InteropServices, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null'
// Found single assembly: 'System.Runtime.InteropServices, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
// WARN: Version mismatch. Expected: '2.1.0.0', Got: '4.1.2.0'
// Load from: 'C:\Program Files\Unity\Hub\Editor\2022.3.36f1\Editor\Data\NetStandard\compat\2.1.0\shims\netstandard\System.Runtime.InteropServices.dll'
// ------------------
// Resolve: 'System.Runtime.CompilerServices.Unsafe, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null'
// Could not find by name: 'System.Runtime.CompilerServices.Unsafe, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null'
// #endif