namespace Egc;

/// <summary>
/// NOU: ceasul aplicației (secțiunea 2). Primește la fiecare cadru timpul REAL scurs
/// (args.Time din OnUpdateFrame) și calculează timpul de JOC (DeltaTime, TotalTime),
/// pe care îl folosesc animațiile: poate fi pus pe pauză, încetinit sau accelerat.
/// </summary>
public sealed class Clock
{
    /// <summary>Cel mai mare pas de timp acceptat, în secunde (vezi secțiunea 2.2).</summary>
    public const float MaxDelta = 0.1f;

    public const float MinScale = 0.125f;
    public const float MaxScale = 4.0f;

    /// <summary>Timpul real scurs de la cadrul anterior, în secunde (nemodificat).</summary>
    public float RealDeltaTime { get; private set; }

    /// <summary>Timpul de joc scurs de la cadrul anterior: limitat, scalat, 0 în pauză.</summary>
    public float DeltaTime { get; private set; }

    /// <summary>Timpul de joc total, în secunde (se oprește în pauză).</summary>
    public double TotalTime { get; private set; }

    /// <summary>Scara timpului: 1 = normal, 0.5 = de două ori mai încet, 2 = de două ori mai repede.</summary>
    public float TimeScale { get; private set; } = 1.0f;

    public bool Paused { get; set; }

    /// <summary>Apelată O DATĂ pe cadru, la începutul lui Game.OnUpdateFrame.</summary>
    public void Tick(double realSeconds)
    {
        RealDeltaTime = (float)realSeconds;

        // TODO 6.1: timpul de joc
        //   1. limitați pasul real la MaxDelta (Math.Min / MathF.Min);
        //   2. în pauză pasul de joc este 0, altfel este pasul limitat înmulțit cu TimeScale.


        float clampedDelta = MathF.Min(RealDeltaTime, MaxDelta);
        //MathF.Mini(a,b) returneazza valoarea mai mica dintre cele doua numere de tip float. 
        //RealDeltaTime este timpul real (in secunde) scurs de la ultimul cadru (frame)
        //Maxdelta este constanta setata la 0.1f(0.1 secunde)
        //daca calculatorul se blocheaza, pentru catevas secunde realdeltatime devine 2.0f dar clampedDelta
        //il va limita la maxim 0.1f pentru ca obiectele din joc ssa nu se teleporteze brucs dintr unb capat in altul al ecranului
        DeltaTime = Paused ? 0f : clampedDelta * TimeScale;   // provizoriu: timpul de joc = timpul real
        //aici se foloseste operatorul ternar car este asemanator cu functia if else:
        //daca Pused este true(joc in pauza), deltatime primeste valoarae 0 si obiectele vor ingheta pe loc
        //daca este false jocul ruleaza: 1.0f timpul din joc trece la fel de repede ca in viata reala, modificarea acestei valori va grabi sau inceta animatiile si pasul de timp
        TotalTime += DeltaTime;
    }

    /// <summary>Înmulțește scara timpului cu <paramref name="factor"/> (de exemplu 2 sau 0.5), între limite.</summary>
    public void ChangeScale(float factor)
    {
        TimeScale = Math.Clamp(TimeScale * factor, MinScale, MaxScale);
    }

    public void ResetScale() => TimeScale = 1.0f;
}
