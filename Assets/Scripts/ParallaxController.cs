using System;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    [Serializable]
    public class ParallaxLayer
    {
        public Transform layerTransform;
        public float speedMultiplier = 0.5f;

        [HideInInspector] public Transform[] instances;
        [HideInInspector] public float textureWidth;
    }

    [SerializeField] private ParallaxLayer[] layers;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;

        for (int i = 0; i < layers.Length; i++)
        {
            SpriteRenderer sr = layers[i].layerTransform.GetComponentInChildren<SpriteRenderer>();
            float width = sr.bounds.size.x;
            layers[i].textureWidth = width;

            layers[i].instances = new Transform[2];
            layers[i].instances[0] = layers[i].layerTransform;
            layers[i].instances[1] = Instantiate(layers[i].layerTransform, layers[i].layerTransform.position + Vector3.right * width, layers[i].layerTransform.rotation, layers[i].layerTransform.parent);
        }
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        foreach (ParallaxLayer layer in layers)
        {
            foreach (Transform instance in layer.instances)
            {
                instance.position += new Vector3(deltaMovement.x * layer.speedMultiplier, 0f, 0f);
            }

            Transform left = layer.instances[0];
            Transform right = layer.instances[1];

            if (cameraTransform.position.x > right.position.x)
            {
                left.position = right.position + Vector3.right * layer.textureWidth;
                Swap(ref layer.instances[0], ref layer.instances[1]);
            }
            else if (cameraTransform.position.x < left.position.x)
            {
                right.position = left.position - Vector3.right * layer.textureWidth;
                Swap(ref layer.instances[0], ref layer.instances[1]);
            }
        }

        lastCameraPosition = cameraTransform.position;
    }

    private void Swap(ref Transform a, ref Transform b)
    {
        Transform temp = a;
        a = b;
        b = temp;
    }
}
