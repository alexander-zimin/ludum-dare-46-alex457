using Godot;
using System;
using SIO = System.IO;
using System.Collections.Generic;

public static class Utils
{
    public static Random Random {get;} = new Random();

    public static Vector2 ClampVec2(Vector2 vec, float lowerLimit, float upperLimit) {
        return new Vector2(
            Mathf.Clamp(vec.x, lowerLimit, upperLimit),
            Mathf.Clamp(vec.y, lowerLimit, upperLimit)
        );
    }

    public static Vector2 LerpVec2(Vector2 from, Vector2 to, float weight) {
        return new Vector2(
            Mathf.Lerp(from.x, to.x, weight),
            Mathf.Lerp(from.y, to.y, weight)
        );
    }

    public static float Min(this Vector2 vec) {
        return Math.Min(vec.x, vec.y);
    }

    public static Vector2 ScaleTextureToWidth(Texture texture, float width) {
        return new Vector2(width, texture.GetHeight() * width / texture.GetWidth());
    }

    public static Vector2 ScaleTextureToHeight(Texture texture, float height) {
        return new Vector2(texture.GetWidth() * height / texture.GetHeight(), height);
    }

    public static void QueueFreeAllChildren(Node node) {
        foreach(Node child in node.GetChildren()) {
            child.QueueFree();
        }
    }

    public static void RemoveAllChildren(Node node) {
        foreach(Node child in node.GetChildren()) {
            node.RemoveChild(child);
            child.QueueFree();
        }
    }

    public static List<string> ListFilesInDirectory(string path){
        var files = new List<string>();
        var directory = new Directory();
        directory.Open(path);
        directory.ListDirBegin(true, true);
        while(true) {
            var file = directory.GetNext();
            if(file == "")
                break;
            if(!file.BeginsWith("."))
                files.Add(SIO.Path.Combine(path, file));
        }
        directory.ListDirEnd();
        return files;
    }
}
