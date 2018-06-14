using UnityEngine;

namespace notacompany
{
    public class Dissolve : MonoBehaviour
    {
        [SerializeField] private Texture2D _noise;
        [SerializeField] private Vector4 _positionStart;
        [SerializeField] private Vector4 _positionEnd;
        [SerializeField] private Color _colorStart;
        [SerializeField] private Color _colorEnd;

        private Renderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();

            _renderer.material.SetVector("_Start", _positionStart);
            _renderer.material.SetVector("_End", _positionEnd);
            _renderer.material.SetTexture("_NoiseTex", _noise);
            _renderer.material.SetColor("_GlowStart", _colorStart);
            _renderer.material.SetColor("_GlowEnd", _colorEnd);
        }

        private void Update()
        {
			_renderer.material.SetFloat("_Progression", Mathf.PingPong(Time.time * 0.5f, 1.0f));
        }
    }
}