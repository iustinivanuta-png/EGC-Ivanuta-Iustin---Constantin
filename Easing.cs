namespace Egc;

/// <summary>Funcție de easing: primește progresul t ∈ [0, 1] și întoarce progresul "modelat".</summary>
public delegate float EasingFunction(float t);

/// <summary>
/// NOU: funcțiile de easing (secțiunea 3). Toate pornesc din 0 și ajung în 1, deci animația
/// începe și se termină în aceleași puncte; diferă doar viteza pe drum.
/// </summary>
public static class Easing
{
    /// <summary>Interpolarea liniară: a pentru t = 0, b pentru t = 1, mijlocul pentru t = 0.5.</summary>
    public static float Lerp(float a, float b, float t) => a + (b - a) * t;

    /// <summary>Viteză constantă.</summary>
    public static float Linear(float t) => t;

    /// <summary>Pornește încet și accelerează. Exemplu complet.</summary>
    public static float EaseIn(float t) => t * t;

    /// <summary>Pornește repede și frânează la sosire.</summary>
    public static float EaseOut(float t)
    {
        // TODO 6.2 (a): 1 - (1 - t)²   (EaseIn "întoarsă")
        float u = 1f - t;
        return 1f - u * u;
   // provizoriu
    }

    /// <summary>Pornește încet, accelerează, apoi frânează la sosire.</summary>
    public static float EaseInOut(float t)
    {
        // TODO 6.2 (a): t² · (3 - 2t) este formula pentru curba smoothstep numata EaseInOut aici care transforma o pregresie liniara intr o miscare de forma S
        return t* t * (3f - 2f * t);   // provizoriu
        //t - parametrul de intrare 
        //la 0 este la pozitia de start,
        //la 0.25 obiectul a parcurs 15,6 la suta din idstanta deci pleaca lent,
        ////la 0.5 atinge viteza maxima la jumatatea drumului
        //la 0.75  mai are 15, 6 % din drim deci franeaza usor
        //la final t = 1.0 se opreste usor fara miscari burste ajungand la destinatie
        //aceasta linie de cod face ca un obiect sa nu se miste mecanic (porneste opreste instantaneu) ci sa aiba inertie (accelereaza treptat)
    }

    /// <summary>Toate funcțiile, cu nume (pentru scena laboratorului și pentru meniuri).</summary>
    public static readonly (string Name, EasingFunction Function)[] All =
    {
        ("Linear", Linear),
        ("EaseIn", EaseIn),
        ("EaseOut", EaseOut),
        ("EaseInOut", EaseInOut),
    };
}
