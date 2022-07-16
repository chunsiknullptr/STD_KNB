using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidActor
{
    public int id;
    public Vector3 position;
    List<int> indexes = new List<int>();

    // size �� ���� 1�� ������ ����

    public List<int> PositionToIndex(out int centerIndex)
    {
        Vector2 toPlane = new Vector2(position.x, position.z);
        int myIndex = (int)(toPlane.y * BoidJob.mapCellX + (int)toPlane.x);
        
        var l = myIndex - 1;
        var r = myIndex + 1;
        var u = myIndex + BoidJob.mapCellX;
        var d = myIndex - BoidJob.mapCellX;
        var lu = u - 1;
        var ru = u + 1;
        var ld = d - 1;
        var rd = d + 1;
        indexes.Clear();
        indexes.Add(myIndex);
        indexes.Add(l);
        indexes.Add(r);
        indexes.Add(u);
        indexes.Add(d);
        indexes.Add(lu);
        indexes.Add(ru);
        indexes.Add(ld);
        indexes.Add(rd);
        centerIndex = myIndex;

#if UNITY_EDITOR
        // Draw 
        var myCharacter = new GameObject("ID " + id);
        myCharacter.transform.position = position;
        DrawCubeForDebug(myIndex, position + Vector3.left, myCharacter.transform);
        DrawCubeForDebug(myIndex, position + Vector3.right, myCharacter.transform);
        DrawCubeForDebug(myIndex, position + Vector3.forward, myCharacter.transform);
        DrawCubeForDebug(myIndex, position + Vector3.back, myCharacter.transform);
#endif

        Debug.Log("---- id : " + id + " ____  pos : " + position);
        foreach (var index in indexes)
        {
            Debug.Log(id + " is cached index to : " + index);
        }
        return indexes;
    }

    private void DrawCubeForDebug(int index, Vector3 pos, Transform parent)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = pos;
        cube.transform.SetParent(parent);
        cube.transform.localScale = Vector3.one * 0.89f;
    }

    private void IndexToPosition()
    {

    }
}

public class BoidJob
{
    // ĳ������ ��ġ�� ������ �� �� �ִ� �ε����� �����Ѵ�.
    // �ֺ� ĳ���͸� �ε��� ������� ������ �� �ִ�.
    public const int mapCellX = 1000;
    public const int mapCellZ = 1000;
    static float mapSize = 1f;

    List<BoidActor> allActor = new List<BoidActor>();
    Dictionary<int, List<int>> indexToActors = new Dictionary<int, List<int>>();

    public void InitActors(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            var newRand = Random.insideUnitCircle * 1000;
            var actor = new BoidActor()
            {
                id = i,
                position = new Vector3(newRand.x, 0, newRand.y)
            };

            allActor.Add(actor);
            AddToIndex(actor);
        }
    }

    private void AddToIndex(BoidActor actor)
    {
        var actorIndexes = actor.PositionToIndex(out var centerIndex);
        if(!indexToActors.TryGetValue(centerIndex, out var indexes))
        {
            indexes = new List<int>();
            indexToActors.Add(actor.id, indexes);
        }

        indexes.Add(actor.id);
    }
}
