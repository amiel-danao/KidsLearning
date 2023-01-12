using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KidsLearning.Assets
{
    public class DependencyExecutor : MonoBehaviour
    {
        [SerializeField] private List<IComponent> components = new List<IComponent>();
        // Use this for initialization
        void Start()
        {

        }
    }
}