using System;
using System.Collections;
using ModelReplacement;
using UnityEngine;

namespace MoreCharacterSelector
{
	public class MRAKULA : BodyReplacementBase
	{
		public bool Blink = true;

		//Required universally
		protected override GameObject LoadAssetsAndReturnModel()
		{
			string model_name = "Akula";
			return Assets.MainAssetBundle.LoadAsset<GameObject>(model_name);
		}

		//Miku mod specific scripts. Delete this if you have no custom scripts to add. 
		protected override void AddModelScripts()
		{
			UseNoPostProcessing = true;
			StartCoroutine(OnIdle());

		}

		protected override void OnEmoteStart(int emoteId)
		{
			replacementModel.GetComponentInChildren<SkinnedMeshRenderer>().SetBlendShapeWeight(7, 0);
			replacementModel.GetComponentInChildren<SkinnedMeshRenderer>().SetBlendShapeWeight(62, 0);
			replacementModel.GetComponentInChildren<SkinnedMeshRenderer>().SetBlendShapeWeight(76, 0);

			replacementModel.GetComponentInChildren<SkinnedMeshRenderer>().SetBlendShapeWeight(5, 0);
			replacementModel.GetComponentInChildren<SkinnedMeshRenderer>().SetBlendShapeWeight(42, 0);
			replacementModel.GetComponentInChildren<SkinnedMeshRenderer>().SetBlendShapeWeight(91, 0);

			if (emoteId == 1)
			{
				replacementModel.GetComponentInChildren<SkinnedMeshRenderer>().SetBlendShapeWeight(5, 100);
				replacementModel.GetComponentInChildren<SkinnedMeshRenderer>().SetBlendShapeWeight(42, 100);
				replacementModel.GetComponentInChildren<SkinnedMeshRenderer>().SetBlendShapeWeight(91, (float)87.5);
			}
			if (emoteId == 2)
			{
				replacementModel.GetComponentInChildren<SkinnedMeshRenderer>().SetBlendShapeWeight(7, 100);
				replacementModel.GetComponentInChildren<SkinnedMeshRenderer>().SetBlendShapeWeight(62, 26);
				replacementModel.GetComponentInChildren<SkinnedMeshRenderer>().SetBlendShapeWeight(76, 100);
			}
			Blink = false;
		}

		protected override void OnEmoteEnd()
		{
			replacementModel.GetComponentInChildren<SkinnedMeshRenderer>().SetBlendShapeWeight(7, 0);
			replacementModel.GetComponentInChildren<SkinnedMeshRenderer>().SetBlendShapeWeight(62, 0);
			replacementModel.GetComponentInChildren<SkinnedMeshRenderer>().SetBlendShapeWeight(76, 0);

			replacementModel.GetComponentInChildren<SkinnedMeshRenderer>().SetBlendShapeWeight(5, 0);
			replacementModel.GetComponentInChildren<SkinnedMeshRenderer>().SetBlendShapeWeight(42, 0);
			replacementModel.GetComponentInChildren<SkinnedMeshRenderer>().SetBlendShapeWeight(91, 0);
			Blink = true;
		}

		protected override void OnDeath()
		{
			foreach (var r in replacementDeadBody.GetComponentsInChildren<SkinnedMeshRenderer>())
			{

				var blinkIndex = r.sharedMesh.GetBlendShapeIndex("EyeClose");
				if (blinkIndex != -1)
				{
					r.SetBlendShapeWeight(blinkIndex, 100f);
					Blink = false;
				}
				else
				{
					Blink = true;
				}
			}
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
					foreach (var r in replacementModel.GetComponentsInChildren<SkinnedMeshRenderer>())
					{
						var blinkIndex = r.sharedMesh.GetBlendShapeIndex("EyeClose");
						if (blinkIndex != -1)
						{
							r.SetBlendShapeWeight(blinkIndex, 100f);
						}
					}

					yield return new WaitForSeconds(0.2f);

					foreach (var r in replacementModel.GetComponentsInChildren<SkinnedMeshRenderer>())
					{
						var blinkIndex = r.sharedMesh.GetBlendShapeIndex("EyeClose");
						if (blinkIndex != -1)
						{
							r.SetBlendShapeWeight(blinkIndex, 0f);
						}
					}

					yield return new WaitForSeconds(15f);
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
		public bool Blink = true;
		protected override GameObject LoadAssetsAndReturnModel()
		{
			string model_name = "pepe";
			return Assets.MainAssetBundle.LoadAsset<GameObject>(model_name);
		}

		protected override void AddModelScripts()
		{
			UseNoPostProcessing = true;
			StartCoroutine(OnIdle());
		}

		protected override void OnEmoteStart(int emoteId)
		{
			foreach (var r in replacementModel.GetComponentsInChildren<SkinnedMeshRenderer>())
			{
				var lockIndex = r.sharedMesh.GetBlendShapeIndex("Lock");
				var sadIndex = r.sharedMesh.GetBlendShapeIndex("Sad");
				if (lockIndex != -1)
				{
					r.SetBlendShapeWeight(lockIndex, 0f);
					if (emoteId == 1)
					{
						r.SetBlendShapeWeight(lockIndex, 100f);
					}
				}
				if (sadIndex != -1)
				{
					r.SetBlendShapeWeight(sadIndex, 0f);
					if (emoteId == 2)
					{
						r.SetBlendShapeWeight(sadIndex, 100f);
					}
				}
			}
			Blink = false;
		}
		protected override void OnEmoteEnd()
		{
			foreach (var r in replacementModel.GetComponentsInChildren<SkinnedMeshRenderer>())
			{
				var lockIndex = r.sharedMesh.GetBlendShapeIndex("Lock");
				var sadIndex = r.sharedMesh.GetBlendShapeIndex("Sad");
				if (lockIndex != -1)
				{
					r.SetBlendShapeWeight(lockIndex, 0f);
				}
				if(sadIndex != -1)
				{
					r.SetBlendShapeWeight(sadIndex, 0f);
				}
			}
			Blink = true;
		}

		protected override void OnDeath()
		{
			foreach (var r in replacementDeadBody.GetComponentsInChildren<SkinnedMeshRenderer>())
			{
				var blinkIndex = r.sharedMesh.GetBlendShapeIndex("CloseEye");
				if (blinkIndex != -1)
				{
					r.SetBlendShapeWeight(blinkIndex, 100f);
					Blink = false;
				}
				else
				{
					Blink = true;
				}
			}
		}

		IEnumerator OnIdle()
		{
			while (true)
			{
				if (Blink == true)
				{
					foreach (var r in replacementModel.GetComponentsInChildren<SkinnedMeshRenderer>())
					{
						var blinkIndex = r.sharedMesh.GetBlendShapeIndex("CloseEye");
						if (blinkIndex != -1)
						{
							r.SetBlendShapeWeight(blinkIndex, 100f);
						}
					}

					yield return new WaitForSeconds(0.3f);

					foreach (var r in replacementModel.GetComponentsInChildren<SkinnedMeshRenderer>())
					{
						var blinkIndex = r.sharedMesh.GetBlendShapeIndex("CloseEye");
						if (blinkIndex != -1)
						{
							r.SetBlendShapeWeight(blinkIndex, 0f);
						}
					}

					yield return new WaitForSeconds(15f);
				}
				else
				{
					yield return null;
				}
			}
		}
	}
}