using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionBox : MonoBehaviour
{
    public Image[] sprites;
    public string[] selections;

    private RectTransform rect;

    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();       
    }

    // Update is called once per frame
    void Update()
    {
        int maxStringLength = 0;
        foreach (string selection in selections) {
            maxStringLength = Mathf.Max(selection.Length, maxStringLength);
        }

        int width = 48 * (Mathf.CeilToInt((maxStringLength - 2) / 3) + 3);
        int height = 48 * selections.Length;

        rect.sizeDelta = new Vector2(width, height);

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Image tile = null;

                if (x == 0 && y == 0) {
                    // create top left;
                    tile = Instantiate(sprites[0], Vector3.zero, Quaternion.identity);
                }
                else if (x == 0 && y == height - 1) 
                {
                    // create bottom left
                    tile = Instantiate(sprites[6], Vector3.zero, Quaternion.identity);
                }
                else if (x == width - 1 && y == 0) 
                {
                    // create top right;
                    tile = Instantiate(sprites[2], Vector3.zero, Quaternion.identity);
                }
                else if (x == width - 1 && y == height - 1) 
                {
                    // create bottom right;
                    tile = Instantiate(sprites[8], Vector3.zero, Quaternion.identity);
                }
                else if (x == 0) {
                    // create top;
                    tile = Instantiate(sprites[1], Vector3.zero, Quaternion.identity);
                }
                else if (y == 0) 
                {
                    // create left;
                    tile = Instantiate(sprites[3], Vector3.zero, Quaternion.identity);
                }
                else if (x == width - 1) 
                {
                    // create right;
                    tile = Instantiate(sprites[5], Vector3.zero, Quaternion.identity);
                }
                else if (y == height - 1) 
                {
                    // create bottom
                    tile = Instantiate(sprites[7], Vector3.zero, Quaternion.identity);
                }
                else
                {
                    // create center
                    tile = Instantiate(sprites[4], Vector3.zero, Quaternion.identity);
                }

                tile.transform.SetParent(transform);
                RectTransform tileRect = tile.GetComponent<RectTransform>();
            }
        }

    }
}
