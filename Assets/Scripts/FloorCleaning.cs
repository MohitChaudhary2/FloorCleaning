using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class FloorCleaning : MonoBehaviour
{
    [SerializeField] private Texture2D dirtMaskTextureBase;
    [SerializeField] private Texture2D dirtBrush;
    [SerializeField] private Material material;
    [SerializeField] private float speed;

    [SerializeField] private FixedJoystick jInput;

    private Texture2D dirtMaskTexture;
    private Rigidbody rb;

    
    private Vector2Int lastPaintPixelPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        dirtMaskTexture = new Texture2D(dirtMaskTextureBase.width, dirtMaskTextureBase.height);
        dirtMaskTexture.SetPixels(dirtMaskTextureBase.GetPixels());
        dirtMaskTexture.Apply();
        material.SetTexture("_DirtMask", dirtMaskTexture);   
    }

    private void Update()
    {
        PlayerMovement();
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit raycastHit))
            {
                Vector2 textureCoord = raycastHit.textureCoord;
                int pixelX = (int)(textureCoord.x * dirtMaskTexture.width);
                int pixelY = (int)(textureCoord.y * dirtMaskTexture.height);

                Vector2Int paintPixelPosition = new Vector2Int(pixelX, pixelY);
          
                int paintPixelDistance = Mathf.Abs(paintPixelPosition.x - lastPaintPixelPosition.x) + Mathf.Abs(paintPixelPosition.y - lastPaintPixelPosition.y);
                int maxPaintDistance = 7;
                if (paintPixelDistance < maxPaintDistance)
                {
                    return;
                }
                lastPaintPixelPosition = paintPixelPosition;


            int pixelXOffset = pixelX - (dirtBrush.width / 2);
            int pixelYOffset = pixelY - (dirtBrush.height / 2);

            for (int x = 0; x < dirtBrush.width; x++)
            {
                for (int y = 0; y < dirtBrush.height; y++)
                {
                    Color pixelDirt = dirtBrush.GetPixel(x, y);
                    Color pixelDirtMask = dirtMaskTexture.GetPixel(pixelXOffset + x, pixelYOffset + y);


                    dirtMaskTexture.SetPixel(
                        pixelXOffset + x,
                        pixelYOffset + y,
                        new Color(0, pixelDirtMask.g * pixelDirt.g, 0)
                    );
                }
            }
            dirtMaskTexture.Apply();
    }
}

    void PlayerMovement()
    {
        
        /*float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");*/
        
        Vector3 direction = new Vector3(jInput.Direction.x, 0, jInput.Direction.y).normalized;
        direction = Vector3.ClampMagnitude(direction, 1);
        rb.velocity = direction * speed;
        //transform.Translate(direction * Time.deltaTime * speed);
    }
}

