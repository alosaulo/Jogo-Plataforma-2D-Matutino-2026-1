# Jogo de Plataforma 2D

Jogo de plataforma 2D feito em Unity pela turma do Matutino 2026/1, na disciplina de lógica de programação.

O jogador atravessa 3 fases coletando moedas, desviando de inimigos e resolvendo puzzles, até enfrentar o chefão, o **Rei Slime**.

## 📖 Tutorial

O passo a passo completo de como o jogo foi construído está em **[TUTORIAL.md](TUTORIAL.md)**.

Ele explica cada parte do jogo (o que fazer no Editor, o código e como testar) e foi escrito para você seguir sozinho, do começo ao fim.

## O que tem no jogo

- Tela inicial com os botões **Jogar** e **Sair**
- **3 fases** com temas diferentes: campo verde, outono e neve
- Moedas, corações de vida e slimes inimigos
- **Espinhos** que tiram vida
- **Plataformas que se movem** e carregam o jogador
- **Puzzle de empurrar caixa**: a caixa em cima do botão abre a porta
- **Chefão** com barra de vida, que fica mais rápido quando perde metade da vida
- Telas de **Game Over** e de **Vitória**

## Controles

| Ação | Tecla |
|---|---|
| Andar | **A / D** ou **setas** |
| Pular | **Espaço** |
| Atirar | **Enter** ou **clique do mouse** |

Também dá para derrotar inimigos pulando em cima deles.

## Como abrir o projeto

1. Instale o **Unity 6000.0.71f1** pelo Unity Hub (use esta versão para evitar problemas).
2. Baixe o projeto: botão verde **Code** → **Download ZIP**, ou `git clone` do repositório.
3. No Unity Hub, clique em **Add** → **Add project from disk** e escolha a pasta do projeto.
4. Abra a cena `Assets/Scenes/Menu` e aperte **Play**.

## Organização

```
Assets/
├── Scenes/      → Menu, Fase1, Fase2, Fase3, Vitoria
├── Scripts/     → todo o código C# (comentado em português)
├── Prefabs/     → slimes, moeda, tiro, espinho, caixa, plataforma móvel
├── Palettes/    → paleta de tiles "Season", para desenhar as fases
├── Sprite/      → imagens do personagem, inimigos e cenário
├── Fonts/       → fonte dos títulos
└── Animations/  → animações do jogador e da moeda
```

Os scripts usam só o básico de C#: variáveis, `if`, métodos e componentes do Unity. Não há herança nem interfaces, para o código ser fácil de ler por quem está começando.

## Tecnologias

- Unity 6 (6000.0.71f1) com URP 2D
- Input System
- TextMeshPro
- Tilemap

## Licença

Distribuído sob a licença MIT. Veja o arquivo [LICENSE](LICENSE).
