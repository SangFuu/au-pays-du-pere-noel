using System;
using System.Collections.Generic;
using System.Linq;

namespace AuPaysDuPereNoel
{
    // ============= ENUMS ET STRUCTURES =============

    public enum TypeCadeau
    {
        Nounours,
        Tricycle,
        Jumelles,
        AbonnementGeekJr,
        Ordinateur
    }

    public enum Continent
    {
        Europe,
        Asie,
        Afrique,
        Amérique,
        Océanie
    }

    public enum StatutEmploye
    {
        EnTravail,
        EnAttente,
        EnRepos
    }

    //                ============= CLASSE LETTRE =============

    public class Lettre
    {
        public string NomEnfant { get; set; }
        public string Adresse { get; set; }
        public Continent Continent { get; set; }
        public int Age { get; set; }

        public Lettre(string nom, string adresse, Continent continent, int age)
        {
            NomEnfant = nom;
            Adresse = adresse;
            Continent = continent;
            Age = age;
        }

        // Méthodes statiques utilitaires
        public static TypeCadeau DeterminerTypeCadeau(int age)
        {
            // Utilisation de && (ET logique) au lieu de &
            if (age >= 0 && age <= 2)
                return TypeCadeau.Nounours;
            else if (age >= 3 && age <= 5)
                return TypeCadeau.Tricycle;
            else if (age >= 6 && age <= 10)
                return TypeCadeau.Jumelles;
            else if (age >= 11 && age <= 15)
                return TypeCadeau.AbonnementGeekJr;
            else if (age >= 16 && age <= 18)
                return TypeCadeau.Ordinateur;
            else
                // Le simple "else" garantit au compilateur qu'on passe ici 
                // si rien d'autre n'a marché.
                throw new ArgumentException("Vous n'avez plus l'âge de fêter Noël...");
        }

        public static int ObtenirDureeFabrication(int age)
        {
            if (age >= 0 & age <= 2)
            {
                return 3;
            }
            else if (age >= 3 & age <= 5)
            {
                return 4;
            }
            else if (age >= 6 & age <= 10)
            {
                return 6;
            }
            else if (age >= 11 & age <= 15)
            {
                return 1;
            }
            else if (age >= 16 & age <= 18)
            {
                return 10;
            }
            else
                throw new ArgumentException($"L'âge {age} est invalide, il doit être entre 0 et 18. Il n'y a donc pas de duree de fabrication.");
        }

        // Propriétés d'instance pour faciliter l'accès
        public TypeCadeau TypeCadeau
        {
            get
            {
                return DeterminerTypeCadeau(Age);
            }
        }

        public int DureeFabrication
        {
            get
            {
                return ObtenirDureeFabrication(Age);
            }
        }
    }

    // ============= CLASSE LUTIN =============

    public class Lutin
    {
        public int Id { get; set; }
        public StatutEmploye Statut { get; set; }
        public Lettre LettreEnCours { get; set; }
        public int HeuresRestantes { get; set; }

        public Lutin(int id)
        {
            Id = id;
            Statut = StatutEmploye.EnAttente;
            HeuresRestantes = 0;
        }

        public void CommencerFabrication(Lettre lettre)
        {
            //Attribution de la lettre au lutin
            LettreEnCours = lettre;
            //Changement du statut du Lutin*
            Statut = StatutEmploye.EnTravail;
            //Initialisation de HeuresRestantes (compte à rebours de la durée nécessaire du jouet).
            //Cela utilise DureeFabrication de la classe Lettre.
            HeuresRestantes = lettre.DureeFabrication;
        }

        public Lettre Travailler()
        {
            if (Statut != StatutEmploye.EnTravail || LettreEnCours == null)
                return null;

            HeuresRestantes--;

            if (HeuresRestantes <= 0)
            {
                Lettre lettreTerminee = LettreEnCours;
                LettreEnCours = null;
                Statut = StatutEmploye.EnAttente;
                return lettreTerminee;
            }

            return null;
        }
    }

    // ============= CLASSE NAIN =============

    public class Nain
    {
        public int Id { get; set; }
        public StatutEmploye Statut { get; set; }
        public Lettre LettreEnCours { get; set; }
        public int HeuresRestantes { get; set; }

        public Nain(int id)
        {
            Id = id;
            Statut = StatutEmploye.EnAttente;
            HeuresRestantes = 0;
        }

        public void CommencerEmballage(Lettre lettre)
        {
            //Attribution de la lettre au lutin
            LettreEnCours = lettre;
            //Changement du statut du Lutin
            Statut = StatutEmploye.EnTravail;
            //Initialisation de HeuresRestantes (compte à rebours de la durée nécessaire du jouet).
            //Cela utilise DureeFabrication de la classe Lettre.
            HeuresRestantes = 2;
        }

        public Lettre Travailler()
        {
            if (Statut != StatutEmploye.EnTravail || LettreEnCours == null)
                return null;

            HeuresRestantes--;

            if (HeuresRestantes <= 0)
            {
                Lettre lettreTerminee = LettreEnCours;
                LettreEnCours = null;
                Statut = StatutEmploye.EnAttente;
                return lettreTerminee;
            }

            return null;
        }
    }

    // ============= CLASSE TRAINEAU =============a

    public class Traineau
    {
        public Continent Continent { get; set; }
        public Stack<Lettre> Lettres { get; set; }
        public int CapaciteMax { get; set; }
        public bool EnVoyage { get; set; }
        public int HeuresVoyageRestantes { get; set; }

        public Traineau(Continent continent, int capacite)
        {
            Continent = continent;
            CapaciteMax = capacite;
            Lettres = new Stack<Lettre>();
            EnVoyage = false;
        }

        public bool PeutCharger()   //Est-ce qu'on peut charger le traineau ?
        {
            // On peut charger SEULEMENT SI :
            // 1. Le traîneau n'est pas parti en voyage
            // 2. ET qu'il n'est pas déjà plein
            return EnVoyage == false && EstPlein() == false;
        }

        public void ChargerLettre(Lettre lettre)    // charger le traineau si c'est possible
        {
            // On vérifie d'abord si c'est possible
            if (PeutCharger())
            {
                // Comme 'Lettres' est une Pile Comme dit dans le sujet, on utilise alors "Push" pour ajouté
                Lettres.Push(lettre);
            }
        }

        public bool EstPlein()  // Le traineau est-il plein ?
        {
            // Le traîneau est plein si le nombre de lettre est égal (ou supérieur) à la capacité max
            return Lettres.Count >= CapaciteMax;
        }

        public void PartirEnVoyage()    // Le départ du traineau.
        {
            if (Lettres.Count > 0)
            {
                EnVoyage = true;
                HeuresVoyageRestantes = 6;
            }
        }

        public List<Lettre> AvancerVoyage()
        {
            if (!EnVoyage) return null;

            HeuresVoyageRestantes--;

            if (HeuresVoyageRestantes <= 0)
            {
                // Retour de voyage, décharger les lettres
                List<Lettre> lettresLivrees = Lettres.ToList(); // Transforme une pile en Liste
                Lettres.Clear();
                EnVoyage = false;
                return lettresLivrees;
            }

            return null;
        }
    }

    // ============= CLASSE ELFE =============

    public class Elfe
    {
        public int Id { get; set; }
        public Continent Continent { get; set; }
        public Traineau Traineau { get; set; }

        public Elfe(int id, Continent continent, int capaciteTraineau)
        {
            Id = id;
            Continent = continent;
            Traineau = new Traineau(continent, capaciteTraineau);
        }

        public bool PeutRecevoirLettre()
        {
            return Traineau.PeutCharger();
        }

        public void RecevoirLettre(Lettre lettre)
        {
            Traineau.ChargerLettre(lettre);

            // Si le traîneau est plein, partir en voyage
            if (Traineau.EstPlein())
            {
                Traineau.PartirEnVoyage();
            }
        }
    }

    // ============= CLASSE ENTREPOT =============

    public class Entrepot
    {
        public Continent Continent { get; set; }
        public Stack<Lettre> Lettres { get; set; }

        public Entrepot(Continent continent)
        {
            Continent = continent;
            Lettres = new Stack<Lettre>();
        }

        public void AjouterLettres(List<Lettre> lettres)
        {
            foreach (Lettre lettre in lettres)
            {
                Lettres.Push(lettre);
            }
        }

        public int[] GetStatistiques()
        {
            // Tableau indexé par TypeCadeau (0=Nounours, 1=Tricycle, etc.)
            int[] stats = new int[5];

            foreach (Lettre lettre in Lettres)
            {
                stats[(int)lettre.TypeCadeau]++;
            }

            return stats;
        }
    }

    // ============= CLASSE SIMULATION =============

    public class Simulation // La classe qui gère la simulation.
    {
        // Paramètres configurables
        public bool RedemarrageDemande { get; set; }
        public int NbLutins { get; set; }
        public int NbNains { get; set; }
        public int NbEnfants { get; set; }
        public int NbLettresParHeure { get; set; }
        public int NbJouetParTraineau { get; set; }

        // Personnel
        public List<Lutin> Lutins { get; set; }
        public List<Nain> Nains { get; set; }
        public List<Elfe> Elfes { get; set; }

        // Files et piles
        public Stack<Lettre> BureauPereNoel { get; set; }
        public Queue<Lettre> FileAttenteFabrication { get; set; }
        public Queue<Lettre> FileAttenteEmballage { get; set; }
        public Queue<Lettre>[] FilesAttenteContinents { get; set; }
        // Entrepôts (un par continent, indexé par l'enum Continent)
        public Entrepot[] Entrepots { get; set; }

        // Temps et coûts
        public int HeureActuelle { get; set; }
        public int JourActuel { get; set; }
        public double CoutTotal { get; set; }
        public List<double> CoutsParHeure { get; set; }

        // Gestion du personnel
        public int DerniereModificationLutins { get; set; }
        public int DerniereModificationNains { get; set; }

        // Générateur aléatoire
        private Random random;

        // La mise en route de la simulation avec la mise à jour de tous les paramètres.
        public Simulation(int nbLutins, int nbNains, int nbEnfants, int nbLettresParHeure, int nbJouetParTraineau)
        {
            if (nbLutins <= 0) throw new ArgumentOutOfRangeException(nameof(nbLutins));
            if (nbNains <= 0) throw new ArgumentOutOfRangeException(nameof(nbNains));
            if (nbEnfants < 0) throw new ArgumentOutOfRangeException(nameof(nbEnfants));
            if (nbLettresParHeure <= 0 || nbLettresParHeure == int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(nbLettresParHeure));
            if (nbJouetParTraineau <= 0) throw new ArgumentOutOfRangeException(nameof(nbJouetParTraineau));
            NbLutins = nbLutins;
            NbNains = nbNains;
            NbEnfants = nbEnfants;
            NbLettresParHeure = nbLettresParHeure;
            NbJouetParTraineau = nbJouetParTraineau;

            random = new Random();
            HeureActuelle = 0;
            JourActuel = 1;
            CoutTotal = 0;
            CoutsParHeure = new List<double>();
            DerniereModificationLutins = -12;   // Pour bien gérer le fait qu'on ne peut recruter des lutins pendant 12 heures si on en a mis en repos.
            DerniereModificationNains = -24;    // Pour bien gérer le fait qu'on ne peut recruter des nains pendant 12 heures si on en a mis en repos.

            // Initialiser les lutins
            Lutins = new List<Lutin>();
            for (int i = 0; i < nbLutins; i++)
            {
                Lutins.Add(new Lutin(i + 1));
            }

            // Initialiser les nains
            Nains = new List<Nain>();
            for (int i = 0; i < nbNains; i++)
            {
                Nains.Add(new Nain(i + 1));
            }

            // Initialiser les elfes (5, un par continent)
            Elfes = new List<Elfe>();
            int elfId = 1;
            foreach (Continent continent in Enum.GetValues(typeof(Continent)))
            {
                Elfes.Add(new Elfe(elfId++, continent, nbJouetParTraineau));
            }

            // Initialiser les structures de données
            BureauPereNoel = new Stack<Lettre>();
            FileAttenteFabrication = new Queue<Lettre>();
            FileAttenteEmballage = new Queue<Lettre>();

            FilesAttenteContinents = new Queue<Lettre>[5];
            for (int i = 0; i < 5; i++)
            {
                FilesAttenteContinents[i] = new Queue<Lettre>();
            }

            Entrepots = new Entrepot[5];
            for (int i = 0; i < 5; i++)
            {
                Entrepots[i] = new Entrepot((Continent)i);
            }

            // Générer toutes les lettres
            GenererLettres();
        }

        private void GenererLettres()
        {
            string[] prenoms = { "Alice", "Bob", "Charlie", "Diana", "Emma", "Frank", "Grace", "Hugo",
                               "Iris", "Jack", "Kate", "Leo", "Marie", "Nathan", "Olivia", "Paul" };
            string[] villes = { "Paris", "Londres", "Tokyo", "New York", "Sydney", "Le Caire",
                              "Berlin", "Rome", "Pékin", "Rio", "Moscou", "Toronto" };

            List<Lettre> toutesLesLettres = new List<Lettre>();

            for (int i = 0; i < NbEnfants; i++)
            {
                string nom = prenoms[random.Next(prenoms.Length)] + i;
                string adresse = villes[random.Next(villes.Length)];
                Continent continent = (Continent)random.Next(5);
                int age = random.Next(0, 19);

                toutesLesLettres.Add(new Lettre(nom, adresse, continent, age));
            }

            // Mélanger les lettres (algorithme Fisher-Yates)
            for (int i = toutesLesLettres.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                Lettre temp = toutesLesLettres[i];
                toutesLesLettres[i] = toutesLesLettres[j];
                toutesLesLettres[j] = temp;
            }

            foreach (Lettre lettre in toutesLesLettres)
            {
                BureauPereNoel.Push(lettre);
            }
        }

        public void AvancerUneHeure()
        {
            double TotalParHeure = 0;
            // ÉTAPE 1 : ARRIVÉE DU COURRIER
            // Chaque heure, un nombre aléatoire de lettres (entre 0 et NbLettresParHeure)
            // Ces lettres sont prises sur le bureau du Père Noël (la pile globale) pour être mises en file de fabrication.
            int nbLettresArrivantes = random.Next(NbLettresParHeure + 1);

            // On ne peut pas traiter plus de lettres qu'il n'y en a réellement sur le bureau
            int quantiteATraiter = Math.Min(nbLettresArrivantes, BureauPereNoel.Count);

            for (int i = 0; i < quantiteATraiter; i++)
            {
                Lettre l = BureauPereNoel.Pop();
                FileAttenteFabrication.Enqueue(l);
            }
            // ÉTAPE 2 : GESTION DES LUTINS (FABRICATION)
            foreach (Lutin lutin in Lutins)
            {
                // Si le lutin est au repos, il ne coûte rien et ne fait rien.
                if (lutin.Statut == StatutEmploye.EnRepos) 
                    continue;

                // --- Cas A : Le lutin travaille déjà ---
                if (lutin.Statut == StatutEmploye.EnTravail)
                {
                    TotalParHeure += 1.5;// Coût : 1.5 pièces d'or par heure de travail.

                    Lettre lettreFinie = lutin.Travailler(); // Avance la fabrication d'une heure.

                    if (lettreFinie != null)
                    {
                        // Le jouet est terminé, il passe aux Nains (file d'emballage).
                        FileAttenteEmballage.Enqueue(lettreFinie);
                    }
                }
                // --- Cas B : Le lutin attend du travail ---
                else if (lutin.Statut == StatutEmploye.EnAttente)
                {
                    TotalParHeure += 1.0;// Coût : 1 pièce d'or par heure d'attente.

                    // S'il y a des lettres en attente, il en prend une immédiatement.
                    if (FileAttenteFabrication.Count > 0)
                    {
                        Lettre aFabriquer = FileAttenteFabrication.Dequeue();
                        lutin.CommencerFabrication(aFabriquer);
                    }
                }
            }
            // ========================================================================
            // ÉTAPE 3 : GESTION DES NAINS (EMBALLAGE)
            // ========================================================================
            foreach (Nain nain in Nains)
            {
                if (nain.Statut == StatutEmploye.EnRepos) continue;

                // --- Cas A : Le nain travaille déjà ---
                if (nain.Statut == StatutEmploye.EnTravail)
                {
                    TotalParHeure += 1.0;// Coût : 1 pièce d'or par heure de travail.

                    Lettre lettreEmballee = nain.Travailler(); // Avance l'emballage d'une heure.

                    if (lettreEmballee != null)
                    {
                        // Le colis est prêt, il est transmis à l'elfe du continent correspondant.
                        int indexContinent = (int)lettreEmballee.Continent;
                        FilesAttenteContinents[indexContinent].Enqueue(lettreEmballee);
                    }
                }
                // --- Cas B : Le nain attend du travail ---
                else if (nain.Statut == StatutEmploye.EnAttente)
                {
                    TotalParHeure += 0.5;// Coût : 0.5 pièce d'or par heure d'attente.

                    // S'il y a des jouets à emballer, il en prend un.
                    if (FileAttenteEmballage.Count > 0)
                    {
                        Lettre aEmballer = FileAttenteEmballage.Dequeue();
                        nain.CommencerEmballage(aEmballer);
                    }
                }
            }
            // ========================================================================
            // ÉTAPE 4 : GESTION DES ELFES (TRANSPORT)
            // ========================================================================
            foreach (Elfe elfe in Elfes)
            {
                Traineau t = elfe.Traineau;

                // --- Cas A : Le traîneau est en vol ---
                if (t.EnVoyage)
                {
                    TotalParHeure += 2.0;// Coût : 2 pièces d'or par heure de voyage.

                    // On fait avancer le voyage. Si la méthode retourne une liste, c'est que le traîneau est revenu.
                    List<Lettre> livraison = t.AvancerVoyage();

                    if (livraison != null)
                    {
                        // Le traîneau est revenu et a livré les cadeaux dans l'entrepôt.
                        int index = (int)elfe.Continent;
                        Entrepots[index].AjouterLettres(livraison);
                    }
                }
                // --- Cas B : L'elfe est au pôle Nord (chargement) ---
                else
                {
                    TotalParHeure += 1.5;// Coût : 1.5 pièce d'or par heure de chargement.

                    // On récupère la file d'attente spécifique au continent de cet elfe
                    Queue<Lettre> fileDuContinent = FilesAttenteContinents[(int)elfe.Continent];

                    // Tant qu'il y a des colis en attente ET de la place dans le traîneau
                    while (fileDuContinent.Count > 0 && t.PeutCharger())
                    {
                        Lettre l = fileDuContinent.Dequeue();
                        elfe.RecevoirLettre(l); // Charge le colis. [cite_start]Si plein, le traîneau partira automatiquement.
                    }

                    // Livrer aussi les derniers colis lorsque le traîneau n'est pas plein.
                    bool productionTerminee = BureauPereNoel.Count == 0
                        && FileAttenteFabrication.Count == 0 && FileAttenteEmballage.Count == 0
                        && Lutins.All(l => l.Statut != StatutEmploye.EnTravail)
                        && Nains.All(n => n.Statut != StatutEmploye.EnTravail);
                    if (productionTerminee && fileDuContinent.Count == 0
                        && !t.EnVoyage && t.Lettres.Count > 0)
                        t.PartirEnVoyage();
                }
            }
            // ========================================================================
            // ÉTAPE 5 : MISE À JOUR DU TEMPS ET DES COÛTS
            // ========================================================================
            CoutTotal += TotalParHeure;// Mise à jour du coût global.
            CoutsParHeure.Add(TotalParHeure); // Historique pour le calcul de la moyenne.
            if (HeureActuelle < 11)
            {
                HeureActuelle++;
            }
            else
            {
                JourActuel++;
                HeureActuelle = 0;
            }
        }

        public bool ToutesLettresTraitees()
        {
            if (BureauPereNoel.Count > 0) return false;
            if (FileAttenteFabrication.Count > 0) return false;
            if (FileAttenteEmballage.Count > 0) return false;

            // Vérifier si des lutins travaillent
            foreach (Lutin lutin in Lutins)
            {
                if (lutin.Statut == StatutEmploye.EnTravail)
                    return false;
            }

            // Vérifier si des nains travaillent
            foreach (Nain nain in Nains)
            {
                if (nain.Statut == StatutEmploye.EnTravail)
                    return false;
            }

            // Vérifier les files d'attente des continents
            foreach (Queue<Lettre> file in FilesAttenteContinents)
            {
                if (file.Count > 0)
                    return false;
            }

            // Vérifier les elfes et traîneaux
            foreach (Elfe elfe in Elfes)
            {
                if (elfe.Traineau.EnVoyage || elfe.Traineau.Lettres.Count > 0)
                    return false;
            }

            return true;
        }

        // Méthode utilitaire pour calculer le temps absolu (en heures) depuis le début.
        // Utile pour comparer les délais entre plusieurs jours.
        private int GetTotalHeures()
        {
            // (Jours précédents * 12 heures) + Heure actuelle du jour
            return ((JourActuel - 1) * 12) + HeureActuelle;
        }

        // Variables d'état pour mémoriser quand a eu lieu la dernière mise au repos
        // On les initialise à une valeur négative pour permettre le recrutement dès le début.
        private int _heureDernierReposLutins = -100;
        private int _heureDernierReposNains = -100;

        public void AvancerJusquaJourSuivant()
        {
            // On calcule l'heure cible (la fin de la journée actuelle).
            // Si on est à l'heure 2, on veut aller à 12. Si on est à 14, on veut aller à 24.
            // La formule (HeureActuelle / 12 + 1) * 12 nous donne le prochain multiple de 12.
            int heuresRestantes = 12 - HeureActuelle;

            // On avance heure par heure tant qu'on n'a pas atteint cette cible
            for (int i = 0; i < heuresRestantes; i++)
            {
                AvancerUneHeure();
            }

            // On change de jour une fois le seuil atteint
            Console.WriteLine($"La journée est finie, Début du Jour {JourActuel}");
        }

        public void ModifierNombreLutins(int nouveauNombre)
        {
            // Vérification de la limite physique : on ne peut dépasser nbLutins.
            if (nouveauNombre < 0 || nouveauNombre > NbLutins)
            {
                Console.WriteLine($"Impossible : Vous ne disposez que de {NbLutins} lutins au maximum.");
                return;
            }

            // Calcul du nombre de lutins actuellement actifs (non EnRepos)
            int nombreActuel = 0;
            foreach (var l in Lutins)
            {
                if (l.Statut != StatutEmploye.EnRepos) nombreActuel++;
            }

            if (nouveauNombre == nombreActuel) return;

            // --- CAS 1 : RECRUTEMENT (Augmentation) ---
            if (nouveauNombre > nombreActuel)
            {
                // Règle : Si on a mis des lutins au repos, on ne peut en rappeler avant 12 heures.
                if (GetTotalHeures() - _heureDernierReposLutins < 12)
                {
                    Console.WriteLine("Impossible : Délai de 12h non respecté depuis la dernière mise au repos.");
                    return;
                }

                int aRecruter = nouveauNombre - nombreActuel;
                foreach (var l in Lutins)
                {
                    if (aRecruter == 0) break;
                    // On réactive les lutins qui dorment
                    if (l.Statut == StatutEmploye.EnRepos)
                    {
                        l.Statut = StatutEmploye.EnAttente;
                        aRecruter--;
                    }
                }
                Console.WriteLine($"{nouveauNombre - nombreActuel} lutin(s) rappelé(s).");
            }
            // --- CAS 2 : MISE AU REPOS (Diminution) ---
            else
            {
                int aRenvoyer = nombreActuel - nouveauNombre;

                // On met au repos en priorité ceux qui ne font rien
                foreach (var l in Lutins)
                {
                    if (aRenvoyer == 0) break;
                    if (l.Statut == StatutEmploye.EnAttente)
                    {
                        l.Statut = StatutEmploye.EnRepos;
                        aRenvoyer--;
                    }
                }

                // Si nécessaire, on interrompt ceux qui travaillent
                if (aRenvoyer > 0)
                {
                    foreach (var l in Lutins)
                    {
                        if (aRenvoyer == 0) break;
                        if (l.Statut == StatutEmploye.EnTravail)
                        {
                            l.Statut = StatutEmploye.EnRepos;
                            if (l.LettreEnCours != null)
                                FileAttenteFabrication.Enqueue(l.LettreEnCours);
                            l.LettreEnCours = null;
                            l.HeuresRestantes = 0;
                            aRenvoyer--;
                        }
                    }
                }

                // On enregistre l'heure pour bloquer les futurs recrutements pendant 12h
                _heureDernierReposLutins = GetTotalHeures();
                Console.WriteLine("Lutins mis au repos. Recrutement bloqué pour 12h.");
            }
            DerniereModificationLutins = GetTotalHeures();
        }

        public void ModifierNombreNains(int nouveauNombre)
        {
            // Vérification de la limite physique : on ne peut dépasser nbNains.
            if (nouveauNombre < 0 || nouveauNombre > NbNains)
            {
                Console.WriteLine($"Impossible : Vous ne disposez que de {NbNains} nains au maximum.");
                return;
            }

            int nombreActuel = 0;
            foreach (var n in Nains)
            {
                if (n.Statut != StatutEmploye.EnRepos) nombreActuel++;
            }

            if (nouveauNombre == nombreActuel) return;

            // --- CAS 1 : RECRUTEMENT (Augmentation) ---
            if (nouveauNombre > nombreActuel)
            {
                // Règle : Si on a mis des nains au repos, on ne peut en rappeler avant 24 heures.
                if (GetTotalHeures() - _heureDernierReposNains < 24)
                {
                    Console.WriteLine("Impossible : Délai de 24h non respecté depuis la dernière mise au repos.");
                    return;
                }

                int aRecruter = nouveauNombre - nombreActuel;
                foreach (var n in Nains)
                {
                    if (aRecruter == 0) break;
                    if (n.Statut == StatutEmploye.EnRepos)
                    {
                        n.Statut = StatutEmploye.EnAttente;
                        aRecruter--;
                    }
                }
                Console.WriteLine($"{nouveauNombre - nombreActuel} nain(s) rappelé(s).");
            }
            // --- CAS 2 : MISE AU REPOS (Diminution) ---
            else
            {
                int aRenvoyer = nombreActuel - nouveauNombre;

                foreach (var n in Nains)
                {
                    if (aRenvoyer == 0) break;
                    if (n.Statut == StatutEmploye.EnAttente)
                    {
                        n.Statut = StatutEmploye.EnRepos;
                        aRenvoyer--;
                    }
                }

                if (aRenvoyer > 0)
                {
                    foreach (var n in Nains)
                    {
                        if (aRenvoyer == 0) break;
                        if (n.Statut == StatutEmploye.EnTravail)
                        {
                            n.Statut = StatutEmploye.EnRepos;
                            if (n.LettreEnCours != null)
                                FileAttenteEmballage.Enqueue(n.LettreEnCours);
                            n.LettreEnCours = null;
                            n.HeuresRestantes = 0;
                            aRenvoyer--;
                        }
                    }
                }

                // On enregistre l'heure pour bloquer les futurs recrutements pendant 24h
                _heureDernierReposNains = GetTotalHeures();
                Console.WriteLine("Nains mis au repos. Recrutement bloqué pour 24h.");
            }
            DerniereModificationNains = GetTotalHeures();
        }

        public void AfficherIndicateurs()
        {
            Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"║  INDICATEURS - Jour {JourActuel}, Heure {HeureActuelle % 12 + 1}/12");
            Console.WriteLine("╠════════════════════════════════════════════════════════════╣");

            // Lutins - compter manuellement
            int lutinsEnTravail = 0;
            int lutinsEnAttente = 0;
            int lutinsEnRepos = 0;
            foreach (Lutin lutin in Lutins)
            {
                if (lutin.Statut == StatutEmploye.EnTravail)
                    lutinsEnTravail++;
                else if (lutin.Statut == StatutEmploye.EnAttente)
                    lutinsEnAttente++;
                else if (lutin.Statut == StatutEmploye.EnRepos)
                    lutinsEnRepos++;
            }

            Console.WriteLine("║ LUTINS:");
            Console.WriteLine($"║   • En travail: {lutinsEnTravail}");
            Console.WriteLine($"║   • En attente: {lutinsEnAttente}");
            Console.WriteLine($"║   • En repos: {lutinsEnRepos}");
            Console.WriteLine($"║   • Cadeaux en fabrication: {lutinsEnTravail}");
            Console.WriteLine($"║   • Lettres en attente: {FileAttenteFabrication.Count}");

            // Nains - compter manuellement
            int nainsEnTravail = 0;
            int nainsEnAttente = 0;
            int nainsEnRepos = 0;
            foreach (Nain nain in Nains)
            {
                if (nain.Statut == StatutEmploye.EnTravail)
                    nainsEnTravail++;
                else if (nain.Statut == StatutEmploye.EnAttente)
                    nainsEnAttente++;
                else if (nain.Statut == StatutEmploye.EnRepos)
                    nainsEnRepos++;
            }

            Console.WriteLine("║ NAINS:");
            Console.WriteLine($"║   • En travail: {nainsEnTravail}");
            Console.WriteLine($"║   • En attente: {nainsEnAttente}");
            Console.WriteLine($"║   • En repos: {nainsEnRepos}");
            Console.WriteLine($"║   • Cadeaux en emballage: {nainsEnTravail}");
            Console.WriteLine($"║   • Cadeaux en attente d'emballage: {FileAttenteEmballage.Count}");

            // Elfes et traîneaux
            Console.WriteLine("║ ELFES ET TRAÎNEAUX:");
            foreach (Elfe elfe in Elfes)
            {
                string statut = elfe.Traineau.EnVoyage ?
                    $"En voyage ({elfe.Traineau.HeuresVoyageRestantes}h restantes)" :
                    $"Au chargement ({elfe.Traineau.Lettres.Count}/{NbJouetParTraineau})";
                int enAttente = FilesAttenteContinents[(int)elfe.Continent].Count;
                Console.WriteLine($"║   • {elfe.Continent}: {statut}, En attente: {enAttente}");
            }

            // Coûts - calculer la moyenne manuellement
            Console.WriteLine("║ COÛTS:");
            Console.WriteLine($"║   • Coût total: {CoutTotal:F2} pièces d'or");

            double coutMoyen = 0;
            if (CoutsParHeure.Count > 0)
            {
                double somme = 0;
                foreach (double cout in CoutsParHeure)
                {
                    somme += cout;
                }
                coutMoyen = somme / CoutsParHeure.Count;
            }
            Console.WriteLine($"║   • Coût moyen/heure: {coutMoyen:F2} pièces d'or");

            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
        }

        public void AfficherBilan()
        {
            Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                     BILAN FINAL");
            Console.WriteLine("╠════════════════════════════════════════════════════════════╣");
            Console.WriteLine($"║ Durée totale: {JourActuel - 1} jours et {HeureActuelle % 12} heures");
            Console.WriteLine($"║ Coût total: {CoutTotal:F2} pièces d'or");

            // Calculer la moyenne manuellement
            double coutMoyen = 0;
            if (CoutsParHeure.Count > 0)
            {
                double somme = 0;
                foreach (double cout in CoutsParHeure)
                {
                    somme += cout;
                }
                coutMoyen = somme / CoutsParHeure.Count;
            }
            Console.WriteLine($"║ Coût moyen par heure: {coutMoyen:F2} pièces d'or");

            Console.WriteLine("╠════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ ENTREPÔTS CONTINENTAUX:");

            for (int i = 0; i < Entrepots.Length; i++)
            {
                Entrepot entrepot = Entrepots[i];
                Console.WriteLine($"║ {entrepot.Continent}:");
                Console.WriteLine($"║   Total: {entrepot.Lettres.Count} jouets");
                int[] stats = entrepot.GetStatistiques();

                // Parcourir chaque type de cadeau
                for (int j = 0; j < stats.Length; j++)
                {
                    if (stats[j] > 0)
                    {
                        TypeCadeau type = (TypeCadeau)j;
                        Console.WriteLine($"║   • {type}: {stats[j]}");
                    }
                }
            }

            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
        }
    }

    // ============= PROGRAMME PRINCIPAL =============

    class Program
    {
        static void Main(string[] args)
        {
            try { Executer(); }
            catch (OperationCanceledException) { Console.WriteLine("\nFin de saisie : simulation arrêtée."); }
        }

        static string LireLigne()
        {
            return Console.ReadLine() ?? throw new OperationCanceledException();
        }

        static int LireEntier(int minimum)
        {
            while (true)
            {
                if (int.TryParse(LireLigne(), out int valeur)
                    && valeur >= minimum && valeur < int.MaxValue)
                    return valeur;
                Console.Write($"Entrez un entier entre {minimum} et {int.MaxValue - 1} : ");
            }
        }

        static void Executer()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Simulation simulation = null;
            bool continuer = true;

            while (continuer)
            {
                if (simulation == null)
                {
                    simulation = MenuInitialisation();
                }
                else
                {
                    continuer = MenuPrincipal(simulation);
                    if (simulation.RedemarrageDemande) { simulation = null; continue; }

                    if (continuer && simulation.ToutesLettresTraitees())
                    {
                        Console.WriteLine("\n🎄 Toutes les lettres ont été traitées! 🎄");
                        simulation.AfficherBilan();

                        Console.Write("\nVoulez-vous lancer une nouvelle simulation? (o/n): ");
                        string choix = LireLigne().ToLower();
                        if (choix == "o" || choix == "oui")
                        {
                            simulation = null;
                        }
                        else
                        {
                            continuer = false;
                        }
                    }
                }
            }

            Console.WriteLine("\n🎅 Joyeux Noël! 🎄");
        }

        static Simulation MenuInitialisation()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║          BIENVENUE AU PAYS DU PÈRE NOËL                    ║");
            Console.WriteLine("║              Initialisation de la simulation               ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            Console.Write("Nombre de lutins disponibles: ");
            int nbLutins = LireEntier(1);

            Console.Write("Nombre de nains disponibles: ");
            int nbNains = LireEntier(1);

            Console.Write("Nombre d'enfants: ");
            int nbEnfants = LireEntier(0);

            Console.Write("Nombre maximum de lettres par heure: ");
            int nbLettresParHeure = LireEntier(1);

            Console.Write("Capacité de chaque traîneau: ");
            int nbJouetParTraineau = LireEntier(1);

            Console.WriteLine("\n✅ Simulation initialisée avec succès!");
            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();

            return new Simulation(nbLutins, nbNains, nbEnfants, nbLettresParHeure, nbJouetParTraineau);
        }

        static bool MenuPrincipal(Simulation simulation)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                 GESTION DU PÈRE NOËL                       ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ 1. Avancer d'une heure                                     ║");
            Console.WriteLine("║ 2. Avancer jusqu'au début de la journée suivante           ║");
            Console.WriteLine("║ 3. Afficher les indicateurs                                ║");
            Console.WriteLine("║ 4. Gérer les lutins                                        ║");
            Console.WriteLine("║ 5. Gérer les nains                                         ║");
            Console.WriteLine("║ 6. Visualiser les entrepôts                                ║");
            Console.WriteLine("║ 7. Afficher le bilan et continuer                          ║");
            Console.WriteLine("║ 8. Afficher le bilan et arrêter                            ║");
            Console.WriteLine("║ 9. Afficher le bilan et relancer                           ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.Write("\nVotre choix: ");

            string choix = LireLigne();

            switch (choix)
            {
                case "1":
                    simulation.AvancerUneHeure();   // Cette Méthode est à écrire.
                    Console.WriteLine($"\n✅ Avancé à: Jour {simulation.JourActuel}, Heure {simulation.HeureActuelle % 12 + 1}");
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return true;

                case "2":
                    int jourAvant = simulation.JourActuel; int heuresAvant = simulation.HeureActuelle;
                    simulation.AvancerJusquaJourSuivant();  // Cette Méthode est à écrire.
                    Console.WriteLine($"\n✅ Avancé jusqu'à: Jour {simulation.JourActuel}, Heure 1");
                    Console.WriteLine($"   ({(simulation.JourActuel - jourAvant) * 12 + simulation.HeureActuelle - heuresAvant} heures écoulées)");
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return true;

                case "3":
                    simulation.AfficherIndicateurs();
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return true;

                case "4":
                    MenuGestionLutins(simulation);
                    return true;

                case "5":
                    MenuGestionNains(simulation);
                    return true;

                case "6":
                    AfficherEntrepots(simulation);
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return true;

                case "7":
                    simulation.AfficherBilan();
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return true;

                case "8":
                    simulation.AfficherBilan();
                    return false;

                case "9":
                    simulation.RedemarrageDemande = true;
                    simulation.AfficherBilan();
                    Console.WriteLine("\n🔄 Redémarrage de la simulation...");
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return false;

                default:
                    Console.WriteLine("❌ Choix invalide.");
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    return true;
            }
        }

        static void MenuGestionLutins(Simulation simulation)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                   GESTION DES LUTINS                       ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            // Compter les lutins actifs
            int actifs = 0;
            foreach (Lutin lutin in simulation.Lutins)
            {
                if (lutin.Statut != StatutEmploye.EnRepos)
                    actifs++;
            }

            Console.WriteLine($"Lutins actuellement actifs: {actifs}/{simulation.NbLutins}");
            Console.WriteLine($"Dernière modification: il y a {(simulation.JourActuel - 1) * 12 + simulation.HeureActuelle - simulation.DerniereModificationLutins} heures");
            Console.WriteLine($"(Modification possible après 12 heures)\n");

            Console.Write("Nouveau nombre de lutins actifs (ou 'c' pour annuler): ");
            string input = LireLigne();

            if (input.ToLower() != "c")
            {
                if (int.TryParse(input, out int nouveau))
                {
                    simulation.ModifierNombreLutins(nouveau);   // Cette Méthode est à écrire.
                }
                else
                {
                    Console.WriteLine("❌ Nombre invalide.");
                }
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        static void MenuGestionNains(Simulation simulation)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                   GESTION DES NAINS                        ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            // Compter les nains actifs
            int actifs = 0;
            foreach (Nain nain in simulation.Nains)
            {
                if (nain.Statut != StatutEmploye.EnRepos)
                    actifs++;
            }

            Console.WriteLine($"Nains actuellement actifs: {actifs}/{simulation.NbNains}");
            Console.WriteLine($"Dernière modification: il y a {(simulation.JourActuel - 1) * 12 + simulation.HeureActuelle - simulation.DerniereModificationNains} heures");
            Console.WriteLine($"(Modification possible après 24 heures)\n");

            Console.Write("Nouveau nombre de nains actifs (ou 'c' pour annuler): ");
            string input = LireLigne();

            if (input.ToLower() != "c")
            {
                if (int.TryParse(input, out int nouveau))
                {
                    simulation.ModifierNombreNains(nouveau);    // Cette Méthode est à écrire.
                }
                else
                {
                    Console.WriteLine("❌ Nombre invalide.");
                }
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        static void AfficherEntrepots(Simulation simulation)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║              ÉTAT DES ENTREPÔTS CONTINENTAUX               ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            for (int i = 0; i < simulation.Entrepots.Length; i++)
            {
                Entrepot entrepot = simulation.Entrepots[i];
                Console.WriteLine($"═══ {entrepot.Continent} ═══");
                Console.WriteLine($"Total de jouets stockés: {entrepot.Lettres.Count}");

                int[] stats = entrepot.GetStatistiques();
                Console.WriteLine("Répartition par catégorie d'âge:");

                // Parcourir les types de cadeaux dans l'ordre de l'enum
                foreach (TypeCadeau type in Enum.GetValues(typeof(TypeCadeau)))
                {
                    int index = (int)type;
                    if (stats[index] > 0)
                    {
                        string categorie;
                        switch (type)
                        {
                            case TypeCadeau.Nounours:
                                categorie = "0-2 ans (Nounours)";
                                break;
                            case TypeCadeau.Tricycle:
                                categorie = "3-5 ans (Tricycle)";
                                break;
                            case TypeCadeau.Jumelles:
                                categorie = "6-10 ans (Jumelles)";
                                break;
                            case TypeCadeau.AbonnementGeekJr:
                                categorie = "11-15 ans (Abonnement)";
                                break;
                            case TypeCadeau.Ordinateur:
                                categorie = "16-18 ans (Ordinateur)";
                                break;
                            default:
                                categorie = "Inconnu";
                                break;
                        }
                        Console.WriteLine($"  • {categorie}: {stats[index]}");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}


