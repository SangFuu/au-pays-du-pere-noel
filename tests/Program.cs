using AuPaysDuPereNoel;
using System.Reflection;

int total = 0;
void Verifier(bool ok, string message) {
    if (!ok) throw new Exception(message);
    total++;
}
foreach (var cas in new[] { (0, TypeCadeau.Nounours, 3), (2, TypeCadeau.Nounours, 3), (3, TypeCadeau.Tricycle, 4), (5, TypeCadeau.Tricycle, 4), (6, TypeCadeau.Jumelles, 6), (10, TypeCadeau.Jumelles, 6), (11, TypeCadeau.AbonnementGeekJr, 1), (15, TypeCadeau.AbonnementGeekJr, 1), (16, TypeCadeau.Ordinateur, 10), (18, TypeCadeau.Ordinateur, 10) }) {
    Verifier(Lettre.DeterminerTypeCadeau(cas.Item1) == cas.Item2, "Type de cadeau incorrect");
    Verifier(Lettre.ObtenirDureeFabrication(cas.Item1) == cas.Item3, "Duree incorrecte");
}
foreach (int age in new[] { -1, 19 }) {
    bool rejete = false;
    try { Lettre.DeterminerTypeCadeau(age); } catch (ArgumentException) { rejete = true; }
    Verifier(rejete, "Age invalide accepte");
}
foreach (int nombre in new[] { 1, 5, 7, 21 }) {
    var simulation = new Simulation(2, 2, 0, 10, 3);
    for (int i = 0; i < nombre; i++)
        simulation.FileAttenteFabrication.Enqueue(new Lettre("Test", "Adresse fictive", (Continent)(i % 5), i % 19));
    int heures = 0;
    while (!simulation.ToutesLettresTraitees() && heures++ < 1000) simulation.AvancerUneHeure();
    Verifier(simulation.ToutesLettresTraitees(), "Livraison bloquee, notamment avec un traineau partiel");
    Verifier(simulation.Entrepots.Sum(e => e.Lettres.Count) == nombre, "Cadeaux perdus ou dupliques");
    Verifier(simulation.CoutsParHeure.Count == heures, "Historique horaire incorrect");
    Verifier(Math.Abs(simulation.CoutTotal - simulation.CoutsParHeure.Sum()) < 0.0001, "Cout total incoherent");
}
foreach (var argsTest in new[] { new[] {0,1,1,1,1}, new[] {1,0,1,1,1}, new[] {1,1,-1,1,1}, new[] {1,1,1,0,1}, new[] {1,1,1,int.MaxValue,1}, new[] {1,1,1,1,0} }) {
    bool rejete = false;
    try { _ = new Simulation(argsTest[0], argsTest[1], argsTest[2], argsTest[3], argsTest[4]); } catch (ArgumentOutOfRangeException) { rejete = true; }
    Verifier(rejete, "Configuration invalide acceptee");
}
var programme = typeof(Simulation).Assembly.GetType("AuPaysDuPereNoel.Program")!;
var lire = programme.GetMethod("LireEntier", BindingFlags.NonPublic | BindingFlags.Static)!;
var entree = Console.In;
try {
    Console.SetIn(new StringReader("abc\n-1\n0\n2147483647\n2\n"));
    Verifier((int)lire.Invoke(null, new object[] {1})! == 2, "Validation des saisies incorrecte");
    Console.SetIn(new StringReader(""));
    bool arrete = false;
    try { lire.Invoke(null, new object[] {1}); } catch (TargetInvocationException ex) when (ex.InnerException is OperationCanceledException) { arrete = true; }
    Verifier(arrete, "Fin de saisie non geree");
} finally { Console.SetIn(entree); }


// Le passage au jour suivant doit terminer sans sauter de jour.
foreach (int heure in new[] {0, 5, 11}) {
    var sim = new Simulation(1,1,0,1,2) { HeureActuelle = heure };
    var avance = Task.Run(() => sim.AvancerJusquaJourSuivant());
    Verifier(avance.Wait(TimeSpan.FromSeconds(2)), "Passage au jour suivant bloque");
    Verifier(sim.JourActuel == 2 && sim.HeureActuelle == 0, "Jour suivant incorrect");
    Verifier(sim.CoutsParHeure.Count == 12-heure, "Nombre d'heures avancees incorrect");
}
foreach (bool lutins in new[] {true, false}) {
    var sim = new Simulation(1,1,0,1,3);
    var lettre = new Lettre("Test", "Fictif", Continent.Europe, 16);
    if (lutins) sim.Lutins[0].CommencerFabrication(lettre);
    else sim.Nains[0].CommencerEmballage(lettre);
    if (lutins) sim.ModifierNombreLutins(0); else sim.ModifierNombreNains(0);
    Verifier((lutins ? sim.FileAttenteFabrication : sim.FileAttenteEmballage).Contains(lettre), "Cadeau perdu lors de la mise au repos");
    if (lutins) sim.ModifierNombreLutins(1); else sim.ModifierNombreNains(1);
    Verifier((lutins ? sim.Lutins[0].Statut : sim.Nains[0].Statut) == StatutEmploye.EnRepos, "Rappel accepte trop tot");
    int delai = lutins ? 12 : 24;
    for(int i=0; i<delai; i++) sim.AvancerUneHeure();
    if (lutins) sim.ModifierNombreLutins(1); else sim.ModifierNombreNains(1);
    Verifier((lutins ? sim.Lutins[0].Statut : sim.Nains[0].Statut) == StatutEmploye.EnAttente, "Rappel refuse apres le delai");
    int limite = 0;
    while(!sim.ToutesLettresTraitees() && limite++<200) sim.AvancerUneHeure();
    Verifier(sim.ToutesLettresTraitees() && sim.Entrepots.Sum(e=>e.Lettres.Count)==1, "Livraison apres rappel incorrecte");
    if (lutins) sim.ModifierNombreLutins(-1); else sim.ModifierNombreNains(-1);
    Verifier((lutins ? sim.Lutins[0].Statut : sim.Nains[0].Statut) == StatutEmploye.EnAttente, "Effectif negatif accepte");
}
Console.WriteLine($"\n{total} verifications reussies.");
