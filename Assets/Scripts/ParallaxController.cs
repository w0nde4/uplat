using System;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    [Serializable]
    public class ParallaxLayer
    {
        public Transform LayerTransform;
        public float SpeedMultiplier = 0.5f;

        [HideInInspector] public Transform[] Instances;
        [HideInInspector] public float TextureWidth;
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
            SpriteRenderer sr = layers[i].LayerTransform.GetComponentInChildren<SpriteRenderer>();
            float width = sr.bounds.size.x;
            layers[i].TextureWidth = width;

            layers[i].Instances = new Transform[2];
            layers[i].Instances[0] = layers[i].LayerTransform;
            layers[i].Instances[1] = Instantiate(layers[i].LayerTransform, layers[i].LayerTransform.position + Vector3.right * width, layers[i].LayerTransform.rotation, layers[i].LayerTransform.parent);
        }
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        foreach (ParallaxLayer layer in layers)
        {
            foreach (Transform instance in layer.Instances)
            {
                instance.position += new Vector3(deltaMovement.x * layer.SpeedMultiplier, 0f, 0f);
            }

            Transform left = layer.Instances[0];
            Transform right = layer.Instances[1];

            if (cameraTransform.position.x > right.position.x)
            {
                left.position = right.position + Vector3.right * layer.TextureWidth;
                Swap(ref layer.Instances[0], ref layer.Instances[1]);
            }
            else if (cameraTransform.position.x < left.position.x)
            {
                right.position = left.position - Vector3.right * layer.TextureWidth;
                Swap(ref layer.Instances[0], ref layer.Instances[1]);
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
