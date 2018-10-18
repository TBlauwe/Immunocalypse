# Compte rendu de la réunion du 18/10

## Personnes présentes
Responsables pédagogiques :
  - Christine LACLEF
  - Adrien DEMILLY

Equipe de développement :
  - Pierre DUBAILLAY
  - Tristan DE BLAUWE
  - Tarek CHOUAKI

## Au programme de la réunion

  - Choisir le type de jeu : Tower Defense ou Clash Royal
  - Faire une première démonstration des avancées produites
  - Faire contrôler la modélisation

## Résumé des discussions

En premier lieu, le type de jeu a été acté : ce sera Clash Royal, avec nos propres
modifications. Plusieurs problèmes de représentation ont été pointés :

  - la représentation des macrophages n'est pas correcte. Les cellules macrophages sont
  de grosses cellules qui se déplacent en "rampant" (plutôt que les mouvements de rotation
  qui ont été implémentés). De plus, il a été précisé que ces derniers n'ont pas de durée
  de vie, ni de limite concernant la vitesse à laquelle les macrophages mangent.
  - le comportement des virus et des bactéries n'est pas correcte lorsque ces derniers
  "attaquent" les cellules : ils doivent ou bien disparaître, ou bien se retrouver dans la
  cellule.

Des corrections sont également nécessaires concernant la manière dont les niveaux se déroulent.
En effet, le jeu doit s'orienter vers des vagues courtes et pauvres en pathogènes, le danger principal
devant venir des cellules infectées. Les cellules deviennent infectées dès lors qu'un pathogène entre
en contact avec elle. Le nombre de pathogènes rencontrés détermine la rapidité entre la phase "cellule infectée"
et la mort de la cellule. Les cellules infectées doivent être mangeables par les macrophages.

Des développements intéressants ont également été abordés concernant la désignation des cibles par le moteur
de jeu. Ce dernier, qui actuellment repose en partie sur des forces et en partie sur des "targets" définies
doit évoluer vers un système de forces complet. Ce dernier apporte une couche de réalisme supplémentaire en mimant
les signaux chimiques envoyés par les différentes cellules, jouant ainsi le rôle d'attracteurs ou de répulseurs.

Concernant le système de cartes, plusieurs points ont été abordés :

  - la gestion du deck est importante et force le joueur à prévoir correctement les défenses qu'il devra mettre en place
  - les cartes dites "bonus" ou "consommables" sont des cartes à effet unique et limité dans le temps. Ces dernières
  sont aussi un moyen de faire passer un message de prévention (antibios, préservatifs ...)
  - Lorsqu'une nouvelle carte est débloquéée, il paraît important que cette dernière puisse être utilisée dans les
  niveaux suivants. Ainsi, il est possible de simuler l'apprentissage par le système immunitaire des différents pathogènes.

## Travail attendu pour la prochaine réunion

Voir le tableau "Sprint 2" sur [Trello](https://trello.com/b/MQlkNWm8/sprint-2).

La prochaine réunion se déroulera la semaine du 5/11/18, le jour et l'heure restent à déterminer.