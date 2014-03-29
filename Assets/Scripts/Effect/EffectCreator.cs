using UnityEngine;
using System.Collections;

public class EffectCreator
{
   private static EffectCreator instance;

   private EffectCreator() {}

   public static EffectCreator Instance
   {
      get 
      {
         if (instance == null)
         {
            instance = new EffectCreator();
         }
         return instance;
      }
   }
	
	public void Effect(Vector3 position, GameObject effectObject){
		GameObject effect = GameObject.Instantiate(effectObject) as GameObject;
		effect.transform.position = position;
	}
}


