/*
	PreviewLabs.RequestParameters

	Public Domain
	
	To the extent possible under law, PreviewLabs has waived all copyright and related or neighboring rights to this document. This work is published from: Belgium.
	
	http://www.previewlabs.com
	
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PreviewLabs
{
	public class RequestParameters : MonoBehaviour
	{
		public string testParameters = "?author=Bernard%20Fran%C3%A7ois&company=PreviewLabs";
	
		private static Dictionary<string, string> parameters = new Dictionary<string, string>(); 
	
		public static bool HasKey(string key)
		{
			return parameters.ContainsKey (key);
		}
			
		// This can be called from Start(), but not earlier
		public static string GetValue(string key)
		{
			return parameters[key];
		}

		public void Awake ()
		{
			Application.ExternalEval(		
				" UnityObject2.instances[0].getUnity().SendMessage('" + name + "', 'SetRequestParameters', document.location.search);"
				);	
	
			#if UNITY_EDITOR
				SetRequestParameters(testParameters);
			#endif
		}
		
		
		public void SetRequestParameters(string parametersString)
		{
			char[] parameterDelimiters = new char[]{'?', '&'};
			string[] parameters = parametersString.Split (parameterDelimiters, System.StringSplitOptions.RemoveEmptyEntries);
			

			char[] keyValueDelimiters = new char[]{'='};
			for (int i=0; i<parameters.Length; ++i)
			{
				string[] keyValue = parameters[i].Split (keyValueDelimiters, System.StringSplitOptions.None);

				if (keyValue.Length >= 2)
				{
					RequestParameters.parameters.Add(WWW.UnEscapeURL(keyValue[0]), WWW.UnEscapeURL(keyValue[1]));
				}
				else if (keyValue.Length == 1)
				{
					RequestParameters.parameters.Add(WWW.UnEscapeURL(keyValue[0]), "");
				}	
			}			
		}
	}
}
