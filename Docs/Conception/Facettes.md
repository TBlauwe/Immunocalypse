# 6 Facettes

## Informations

### Descriptions des buts à atteindre

- Vision du produit (Peaufiner Pitch)

Fonctionnalités   | Poids
---               | ---
Fonctionnalités 1 | 1
Fonctionnalités 2 | 1
Fonctionnalités 3 | 1
Fonctionnalités 4 | 1
Fonctionnalités 5 | 1

Exigences  |
---        |
Exigence 1 |
Exigence 2 |
Exigence 3 |
Exigence 4 |
Exigence 5 |
Exigence 6 |

### Descriptions des moyens envisagés 

### Début de planification

- Définir des itérations

MileStone     | Fonctionnalités
---           | ---
Moteur de jeu | FyFy, ...

## Déroulement du jeu

Au démarrage du jeu, le joueur accède à un menu lui permettant de :

  - Commencer une nouvelle partie
  - Charger une partie
  - De quitter le jeu
  - ...

Lors du démarrage d'une nouvelle partie, le joueur est informé des objectifs, c'est à dire de la nature pédagogique
du jeu. Une fois ce message lu, un écran résumant les différents niveaux accessibles.

Chaque niveau est composé d'un ou plusieurs chemins menant à un ou plusieurs objectifs à défendre. Le long de ces chemins
, le joueur peut disposer des "tours" (plutôt différents types de cellules) pour attaquer les ennemis qui emprunterons le-dit chemin.

Le joueur doit faire face à une ou plusieurs vagues d'ennemis (des pathogènes). Ces ennemis appartiennent à des familles différentes :

  - les bactéries
  - les virus
  - les parasites
  - les cellules cancéreuses
  - les allergènes
  - ...

Chacune de ces familles contiennent plusieurs sous-types (plusieurs types de virus, de bactéries ...). Afin d'inciter le joueur à poursuivre
l'expérience, chaque niveau donne lieu à des récompenses (?) en fonction de divers objectifs (pts de vie restant, argent dépensé ...).

## 6 Facettes

### Objectifs pédagogiques

#### Connaissances
- Expliquer le système immunitaire 
- Que l'apprenant-joueur puisse distinguer les différents éléments du système immunitaires:
    - Cellules
    - Mécanismes
- Les différents pathogènes et maladies et la réponse immunitaire associée.
- Coordination des différentes actions et leur adaptation au pathogène
- Aptitude à reconnaitres les unités immunitaires et pathogènes
- Identifier la réponse immunitaire face aux pathogènes/maladies

#### Idées reçues que l'on veut effacer

Les antibiotiques fonctionnent sur les virus.
etc ...

### Simulation du domaine

Le modèle formel regroupe :

  - l'ensemble des pathogènes définis plus haut (pour rappel, les bactéries, virus, allergènes, cellules cancéreuses ...) modélisés
  sous une forme réaliste (2D ou 3D ?)
  - un ensemble (à définir) de cellules responsables de la défense immunitaire (Lymphocytes, Macrophages, ...) modélisées de
  manière réaliste (2D ou 3D ?)
  - les mécanismes et la chaine immunitaire (renforcement des capacité des Lymphocytes T en présence de cytokine, macrophages qui
  "mangent" les corps étrangers et les déchets, Lymphocytes T qui attaquent avec des immunoglobines ...)
  - la fièvre engendrée par les "combats"
  - une représentation des maladies auto-immunes
  - ...


### Interactions avec le modèle
Nous allons utiliser les mécanismes d'un Tower Defense (Voir Pitch pour plus d'informations).

  - placement des tours (= des cellules du système immunitaire)
  - menu de contruction ?
  - possibilité d'actionner des bonus temporaires

Possibilité de faire un Clash Royale like qui semble mieux cadrer les objectifs pédagogiques, tout en limitant les idées reçues.

### Problèmes et progression
L'apprenant-joueur devra passer plusieurs niveaux, où il devra déployer une réponse immunitaire adapté aux pathogènes et maladies rencontrées.
La difficulté des niveaux est basée sur :

  - le nombre de pathogènes types confondus)
  - le nombre de types différents de pathogènes (virus, bactéries ...)
  - le nombre de chemins empruntable par les ennemis
  - le nombre d'objectifs à défendre
  - éventuellement le nombre de bonus dispos
  - vitesse des pathogènes
  - résistance des pathogènes
  - disponibilité des différentes cellules immunitaires

### Décorum

- Gallerie des entités
- Etoiles pour les niveaux en fonction de l'efficacité de la défense (Temps, ressources dépensées, Vies)
- Eventuellement petit pitch sur les personnes dont on défend le corps

### Conditions d'utilisation

Le jeu doit être auto-suffisant et donc permettre à l'apprenant-joueur de jouer en toute autonomie. 
