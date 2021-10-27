using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Token", menuName = "Token")]
public class TokenBlueprint : MonoBehaviour
{
    public Token ID;
}

public struct Token
{
    public int ID;
}
