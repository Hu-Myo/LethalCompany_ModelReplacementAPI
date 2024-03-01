using System;
using System.Collections;
using ModelReplacement;
using UnityEngine;

namespace MoreCharacterSelector
{
    public static class SkinnedMeshComponentFinder
    {
        public static SkinnedMeshRenderer GetSkinnedMeshRendererByObjectName(string modelName, string objectName, GameObject replacementModel)
        {
            Console.WriteLine($"[MoreCharacterSelector] [Model:{modelName}] Trying Get SkinnedMeshRenderer from GetComponentInChildren()");
            var meshRenderer = replacementModel.GetComponentInChildren<SkinnedMeshRenderer>();
            if (meshRenderer.name != objectName)
            {
                Console.WriteLine($"[MoreCharacterSelector] [Model:{modelName}] Trying Get SkinnedMeshRenderer from Components Array...");
                foreach (var r in replacementModel.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    if (r.name == objectName)
                    {
                        meshRenderer = r;
                        return meshRenderer;
                    }
                }
            }

            if (!meshRenderer || meshRenderer.name != objectName)
            {
                Console.WriteLine($"[MoreCharacterSelector] [Model:{modelName}] Getting SkinnedMeshRenderer by ObjectName({objectName}) Failed!");
            }

            return meshRenderer;
        }

        public static SkinnedMeshRenderer GetSkinnedMeshRendererByBlendShapeName(string modelName, string blendShapeName, GameObject replacementModel, out int blendShapeIndex)
        {
            Console.WriteLine($"[MoreCharacterSelector] [Model:{modelName}] Trying SkinnedMeshRenderer from Components Array...");
            SkinnedMeshRenderer meshRenderer = null;
            var index = -1;
            foreach (var r in replacementModel.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                index = r.sharedMesh.GetBlendShapeIndex(blendShapeName);
                if (index != -1)
                {
                    meshRenderer = r;
                    blendShapeIndex = index;
                    return meshRenderer;
                }
            }

            if (meshRenderer == null || !meshRenderer || meshRenderer.sharedMesh.GetBlendShapeIndex(blendShapeName) == -1)
            {
                Console.WriteLine($"[MoreCharacterSelector] [Model:{modelName}] Getting SkinnedMeshRenderer by BlendShapeName({blendShapeName}) Failed!");
            }

            blendShapeIndex = index;
            return meshRenderer;
        }
    }

    public class MRAKULA : BodyReplacementBase
    {
        private const string MODEL_NAME = "Akula";

        public bool Blink = true;
        private SkinnedMeshRenderer _meshRenderer;
        private static WaitForSeconds _eyeCloseWait = new WaitForSeconds(0.2f), _eyeOpenWait = new WaitForSeconds(15);
        private int _blinkIndex;

        //Required universally
        protected override GameObject LoadAssetsAndReturnModel()
        {
            return Assets.MainAssetBundle.LoadAsset<GameObject>(MODEL_NAME);
        }

        protected override void Start()
        {
            base.Start();
            _meshRenderer = SkinnedMeshComponentFinder.GetSkinnedMeshRendererByBlendShapeName(MODEL_NAME, "EyeClose", replacementModel, out _blinkIndex);
            if (_blinkIndex == -1)
            {
                Console.WriteLine($"[MoreCharacterSelector] [Model:{MODEL_NAME}] Cannot Find BlendShapeIndex [EyeClose]");
                return;
            }

            Console.WriteLine($"[MoreCharacterSelector] [Model:{MODEL_NAME}] Initialized Successfully!");
            StartCoroutine(OnIdle());
        }

        //Miku mod specific scripts. Delete this if you have no custom scripts to add. 
        protected override void AddModelScripts()
        {
            UseNoPostProcessing = true;
        }

        protected override void OnEmoteStart(int emoteId)
        {
            _meshRenderer.SetBlendShapeWeight(7, 0);
            _meshRenderer.SetBlendShapeWeight(62, 0);
            _meshRenderer.SetBlendShapeWeight(76, 0);

            _meshRenderer.SetBlendShapeWeight(5, 0);
            _meshRenderer.SetBlendShapeWeight(42, 0);
            _meshRenderer.SetBlendShapeWeight(91, 0);

            if (emoteId == 1)
            {
                _meshRenderer.SetBlendShapeWeight(5, 100);
                _meshRenderer.SetBlendShapeWeight(42, 100);
                _meshRenderer.SetBlendShapeWeight(91, 87.5f);
            }
            else if (emoteId == 2)
            {
                _meshRenderer.SetBlendShapeWeight(7, 100);
                _meshRenderer.SetBlendShapeWeight(62, 26);
                _meshRenderer.SetBlendShapeWeight(76, 100);
            }
            Blink = false;
        }

        protected override void OnEmoteEnd()
        {
            _meshRenderer.SetBlendShapeWeight(7, 0);
            _meshRenderer.SetBlendShapeWeight(62, 0);
            _meshRenderer.SetBlendShapeWeight(76, 0);

            _meshRenderer.SetBlendShapeWeight(5, 0);
            _meshRenderer.SetBlendShapeWeight(42, 0);
            _meshRenderer.SetBlendShapeWeight(91, 0);
            Blink = true;
        }

        protected override void OnDeath()
        {
            _meshRenderer.SetBlendShapeWeight(_blinkIndex, 100);
            Blink = false;
        }
        /*protected override void OnDeath()
        {
            foreach (var r in replacementModel.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                var smileIndex = r.sharedMesh.GetBlendShapeIndex("Smile");
                var surpriseIndex = r.sharedMesh.GetBlendShapeIndex("Surprise");
                if (smileIndex != -1) {
                    r.SetBlendShapeWeight(smileIndex, 100f);
                }
                if (surpriseIndex != -1) {
                    r.SetBlendShapeWeight(surpriseIndex, 100f);
                }
            }
        }*/

        IEnumerator OnIdle()
        {
            while (true)
            {
                if (Blink == true)
                {
                    _meshRenderer.SetBlendShapeWeight(_blinkIndex, 100);
                    yield return _eyeCloseWait;

                    _meshRenderer.SetBlendShapeWeight(_blinkIndex, 0);
                    yield return _eyeOpenWait;
                }
                else
                {
                    yield return null;
                }
            }
        }
    }

    public class MRPEPE : BodyReplacementBase
    {
        private const string MODEL_NAME = "pepe";

        public bool Blink = true;
        private SkinnedMeshRenderer _blinkMeshRenderer, _lockMeshRenderer, _sadMeshRenderer;
        private static WaitForSeconds _eyeCloseWait = new WaitForSeconds(0.3f), _eyeOpenWait = new WaitForSeconds(15);
        private int _blinkIndex, _lockIndex, _sadIndex;

        protected override GameObject LoadAssetsAndReturnModel()
        {
            return Assets.MainAssetBundle.LoadAsset<GameObject>(MODEL_NAME);
        }

        protected override void Start()
        {
            base.Start();
            _blinkMeshRenderer = SkinnedMeshComponentFinder.GetSkinnedMeshRendererByBlendShapeName(MODEL_NAME, "CloseEye", replacementModel, out _blinkIndex);
            _lockMeshRenderer = SkinnedMeshComponentFinder.GetSkinnedMeshRendererByBlendShapeName(MODEL_NAME, "Lock", replacementModel, out _lockIndex);
            _sadMeshRenderer = SkinnedMeshComponentFinder.GetSkinnedMeshRendererByBlendShapeName(MODEL_NAME, "Sad", replacementModel, out _sadIndex);

            if (_blinkIndex == -1 || _lockIndex == -1 || _sadIndex == -1)
            {
                Console.WriteLine($"[MoreCharacterSelector] [Model:{MODEL_NAME}] Cannot Find BlendShapeIndex(s)");
                return;
            }

            Console.WriteLine($"[MoreCharacterSelector] [Model:{MODEL_NAME}] Initialized Successfully!");
            StartCoroutine(OnIdle());
        }

        protected override void AddModelScripts()
        {
            UseNoPostProcessing = true;
        }

        protected override void OnEmoteStart(int emoteId)
        {
            if (emoteId == 1)
            {
                _lockMeshRenderer.SetBlendShapeWeight(_lockIndex, 100);
            }
            else
            {
                _lockMeshRenderer.SetBlendShapeWeight(_lockIndex, 0);
            }

            if (emoteId == 2)
            {
                _sadMeshRenderer.SetBlendShapeWeight(_sadIndex, 100);
            }
            else
            {
                _sadMeshRenderer.SetBlendShapeWeight(_sadIndex, 0);
            }

            Blink = false;
        }

        protected override void OnEmoteEnd()
        {
            _lockMeshRenderer.SetBlendShapeWeight(_lockIndex, 0);
            _sadMeshRenderer.SetBlendShapeWeight(_sadIndex, 0);
            Blink = true;
        }

        protected override void OnDeath()
        {
            _blinkMeshRenderer.SetBlendShapeWeight(_blinkIndex, 100);
            Blink = false;
        }

        IEnumerator OnIdle()
        {
            while (true)
            {
                if (Blink == true)
                {
                    _blinkMeshRenderer.SetBlendShapeWeight(_blinkIndex, 100);
                    yield return _eyeCloseWait;

                    _blinkMeshRenderer.SetBlendShapeWeight(_blinkIndex, 0);
                    yield return _eyeOpenWait;
                }
                else
                {
                    yield return null;
                }
            }
        }
    }
}