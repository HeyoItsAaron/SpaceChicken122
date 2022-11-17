using UnityEngine;

namespace VRShooterKit
{
    public class ScopeCamera : MonoBehaviour
    {
        [SerializeField] private RenderTexture textureTemplate = null;
        [SerializeField] private Camera scopeCamera = null;
        [SerializeField] private MeshRenderer scopeRender = null;

        private RenderTexture localTexture = null;
        
        private void Awake()
        {
            localTexture = new RenderTexture(textureTemplate);
            localTexture.Create();

            scopeCamera.targetTexture = localTexture;
            scopeRender.materials[0].mainTexture = localTexture;
        }
    }

}

