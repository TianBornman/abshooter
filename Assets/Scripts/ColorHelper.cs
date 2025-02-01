using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public static class ColorHelper
{
    public static List<KeyValuePair<Color, Color>> ColorPairs { get; } = new List<KeyValuePair<Color, Color>>()
    {
        new(new Color(0.6f, 0.2f, 0.4f), new Color(0.2f, 0.8f, 0.6f)),  // Purple & Teal
        new(new Color(0.2f, 0.7f, 0.9f), new Color(0.9f, 0.7f, 0.2f)),  // Sky Blue & Amber
        new(new Color(0.3f, 0.8f, 0.2f), new Color(0.8f, 0.3f, 0.2f)),  // Green & Red
        new(new Color(0.7f, 0.2f, 0.8f), new Color(0.9f, 0.6f, 0.3f)),  // Violet & Gold
        new(new Color(0.5f, 0.5f, 1f), new Color(1f, 0.5f, 0.5f)),      // Blue & Light Red
        new(new Color(0.2f, 0.9f, 0.7f), new Color(0.9f, 0.2f, 0.7f)),  // Mint & Fuchsia
        new(new Color(1f, 0.6f, 0.2f), new Color(0.2f, 0.6f, 1f)),      // Orange & Blue
        new(new Color(0.5f, 0.1f, 0.1f), new Color(0.7f, 0.9f, 0.7f)),  // Dark Red & Pale Green
        new(new Color(0.3f, 0.5f, 0.2f), new Color(0.9f, 0.8f, 0.3f)),  // Olive Green & Yellow
        new(new Color(0.1f, 0.8f, 0.9f), new Color(0.9f, 0.1f, 0.6f)),  // Turquoise & Magenta
        new(new Color(0.8f, 0.2f, 0.2f), new Color(0.5f, 0.5f, 1f)),    // Red & Light Blue
        new(new Color(0.1f, 0.6f, 0.3f), new Color(1f, 1f, 0f)),        // Dark Green & Yellow
        new(new Color(0.4f, 0.4f, 1f), new Color(1f, 0.4f, 0.4f)),      // Light Blue & Red
        new(new Color(0.8f, 0.8f, 0.4f), new Color(0.4f, 0.8f, 0.8f)),  // Yellow & Light Teal
        new(new Color(0.1f, 0.9f, 0.3f), new Color(0.7f, 0.2f, 0.9f)),  // Light Green & Purple
        new(new Color(0.3f, 0.6f, 0.7f), new Color(1f, 0.8f, 0.3f)),    // Blue & Amber
        new(new Color(0.4f, 0.2f, 0.6f), new Color(0.8f, 0.4f, 0.2f)),  // Purple & Orange
        new(new Color(0.7f, 0.7f, 0.7f), new Color(0.2f, 0.2f, 0.2f)),  // Light Grey & Dark Grey
        new(new Color(0.2f, 0.6f, 0.3f), new Color(1f, 1f, 0f)),        // Green & Yellow
    };
}