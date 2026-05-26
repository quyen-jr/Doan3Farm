using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResponsiveGridBag : MonoBehaviour
{
    [SerializeField] GameObject gridParent;

    private float initialWidth = 534f; // Chiều rộng mặc định của grid layout ở 1920x1080
    private float initialHeight = 1027.5f; // Chiều cao mặc định của grid layout ở 1920x1080
    private float gridWidth;
    private float gridHeight;

    // Các thuộc tính của GridLayoutGroup
    private GridLayoutGroup gridLayoutGroup;
    private float initialCellWidth;
    private float initialCellHeight;
    private float initialSpacingX;
    private float initialSpacingY;

    void Start()
    {
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        SetUpGridValues();
        Reponsive();
    }

    private void SetUpGridValues()
    {
        initialCellWidth = gridLayoutGroup.cellSize.x;
        initialCellHeight = gridLayoutGroup.cellSize.y;
        initialSpacingX = gridLayoutGroup.spacing.x;
        initialSpacingY = gridLayoutGroup.spacing.y;
    }

    void Update()
    {
        Reponsive();
    }

    private void Reponsive()
    {
        gridWidth = gridParent.GetComponent<RectTransform>().rect.width;
        gridHeight = gridParent.GetComponent<RectTransform>().rect.height;

        // Tính toán khoảng cách mới cho cả chiều rộng và chiều cao
        float spacingX = initialSpacingX * gridWidth / initialWidth;
        float spacingY = initialSpacingY * gridHeight / initialHeight;

        // Đảm bảo khoảng cách không vượt quá giá trị ban đầu
   //     spacingX = Mathf.Min(spacingX, initialSpacingX);
      //  spacingY = Mathf.Min(spacingY, initialSpacingY);

        // Áp dụng khoảng cách mới
        gridLayoutGroup.spacing = new Vector2(spacingX, spacingY);

        // Tính toán kích thước ô mới cho cả chiều rộng và chiều cao
        float newCellWidth = gridWidth / initialWidth * initialCellWidth;
        float newCellHeight = gridHeight / initialHeight * initialCellHeight;

        // Đảm bảo kích thước ô không vượt quá giá trị ban đầu
        // newCellWidth = Mathf.Min(newCellWidth, initialCellWidth);
        // newCellHeight = Mathf.Min(newCellHeight, initialCellHeight);

        // Áp dụng kích thước ô mới
        gridLayoutGroup.cellSize = new Vector2(newCellWidth, newCellHeight);
    }
}
