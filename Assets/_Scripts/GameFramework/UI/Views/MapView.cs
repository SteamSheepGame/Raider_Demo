using UnityEngine;
using UnityEngine.EventSystems;
namespace Demo.Core
{
    public class MapView: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public RectTransform map;         // 你的大图
        public RectTransform viewPort;    // 显示窗口区域

        private Vector2 lastMousePos;

        public void OnBeginDrag(PointerEventData eventData)
        {
            lastMousePos = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 delta = eventData.position - lastMousePos;
            lastMousePos = eventData.position;

            map.anchoredPosition += delta;

            ClampToWindow();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        void ClampToWindow()
        {
            Vector3 pos = map.anchoredPosition;

            float maxX = (map.rect.width - viewPort.rect.width) / 2f;
            float maxY = (map.rect.height - viewPort.rect.height) / 2f;

            pos.x = Mathf.Clamp(pos.x, -maxX, maxX);
            pos.y = Mathf.Clamp(pos.y, -maxY, maxY);

            map.anchoredPosition = pos;
        }
    }
}