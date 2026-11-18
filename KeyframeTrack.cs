using OpenTK.Mathematics;

namespace Egc;

/// <summary>
/// NOU: o pistă de cadre-cheie (secțiunea 4): o listă de valori-cheie, la intervale EGALE de timp
/// (SecondsPerKey). Între două chei valoarea se interpolează. După ultima cheie pista revine
/// la prima și o ia de la capăt (bucla se închide singură).
/// </summary>
public sealed class KeyframeTrack
{
    private readonly List<Vector3> _keys = new();

    /// <summary>Timpul dintre două chei consecutive, în secunde.</summary>
    public float SecondsPerKey { get; }

    /// <summary>Easing-ul aplicat între două chei (implicit liniar).</summary>
    public EasingFunction Ease = Easing.Linear;

    public KeyframeTrack(float secondsPerKey)
    {
        SecondsPerKey = MathF.Max(secondsPerKey, 0.001f);
    }

    public int Count => _keys.Count;
    public IReadOnlyList<Vector3> Keys => _keys;

    /// <summary>Durata unui tur complet (inclusiv drumul de la ultima cheie înapoi la prima).</summary>
    public float Duration => _keys.Count * SecondsPerKey;

    /// <summary>Adaugă o cheie la sfârșitul pistei.</summary>
    public KeyframeTrack Add(Vector3 value)
    {
        _keys.Add(value);
        return this;   // permite scrierea în lanț: track.Add(...).Add(...)
    }

    public void Clear() => _keys.Clear();

    /// <summary>Valoarea pistei la momentul <paramref name="time"/> (în secunde).</summary>
    public Vector3 Sample(float time)
    {
        if (_keys.Count == 0) return Vector3.Zero;
        if (_keys.Count == 1) return _keys[0];

        // TODO 6.3 (secțiunea 4.2):
        //   1. position = time / SecondsPerKey, adus în [0, Count) cu %   (de exemplu 1.5)
        //   2. i = partea întreagă (cheia de plecare), u = partea fracționară (cât din drum)
        //   3. Lerp între cheia i și cheia următoare ((i + 1) % Count), cu Ease aplicat pe u
        float position =(time / SecondsPerKey) % _keys.Count;
        //imparte timpul total sa durata unui segment ca afle la a cata statie a ajuns trenul ( de ex 1.5 inseamna a trecut de statia 1 si e la jumatatea drumului spre statia 2);
        //keys.count (modulo) face ca traseul sa reia de la 0 cand se termina toate statiile (circuit inchis)
        if (position < 0f) position += _keys.Count;

        int i = (int)position;
        // int i numarul statiei de unde a plecat trenul
        float u = position - i;
        //ia doar partea fractionara si este procentul de la 0 la 1 caqre arata cit din frumul dintre cele doua statiia parcus.
        int next = (i + 1) % _keys.Count;
        //afla numarul urmatoarei statii si jla ultima statie urmatoarea va fi prima din nou cu %_keys.Count


        return Vector3.Lerp(_keys[i], Keys[next], Ease(u));


    }
}
