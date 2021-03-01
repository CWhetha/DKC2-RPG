using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class KongLettersManifest
{
    static List<KLetters> kongLetters;

    public static List<KLetters> KongLetters
    {
        get { return kongLetters; }
        set { kongLetters = value; }
    }


    public static void Init(int amount)
    {
        KongLetters = new List<KLetters>();
        for (int i = 0; i < amount; ++i)
        {
            KLetters kong = new KLetters();
            kong.Init();
            KongLetters.Add(kong);
        }
    }
}
public class KLetters
{
    [SerializeField] bool k;
    [SerializeField] bool o;
    [SerializeField] bool n;
    [SerializeField] bool g;

    public bool K
    {
        get { return k; }
    }

    public bool O
    {
        get { return o; }
    }

    public bool N
    {
        get { return n; }
    }

    public bool G
    {
        get { return g; }
    }

    public void Init()
    {
        k = false;
        o = false;
        n = false;
        g = false;
    }

    public void collectLetter(Letter l)
    {
        if (l == Letter.K)
        {
            k = true;
        }
        else if (l == Letter.O)
        {
            o = true;
        }
        else if (l == Letter.N)
        {
            n = true;
        }
        else if (l == Letter.G)
        {
            g = true;
        }
    }

    public bool getLetter(Letter l)
    {
        if (l == Letter.K)
        {
            return k;
        }
        else if (l == Letter.O)
        {
            return o;
        }
        else if (l == Letter.N)
        {
            return n;
        }
        else if (l == Letter.G)
        {
            return g;
        }
        else
        {
            return false;
        }
    }

    public bool collectedAll()
    {
        if (k && o && n && g)
        {
            return true;
        }
        return false;
    }

}

public enum Letter
{
    K,O,N,G
}
