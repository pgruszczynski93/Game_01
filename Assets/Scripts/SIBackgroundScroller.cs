using UnityEngine;

namespace SpaceInvaders
{
    [RequireComponent(typeof(MeshRenderer))]
    public class SIBackgroundScroller : MonoBehaviour, IMoveable
    {

        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Vector2 _scrollOffset;

        public void MoveObj()
        {
            if (_meshRenderer == null)
            {
                return;
            }
            _meshRenderer.material  .mainTextureOffset += _scrollOffset * Time.deltaTime;
        }


        /// <summary>
        /// add to game started event
        /// </summary>
        private void Update()
        {
            MoveObj();
        }
    }
}

