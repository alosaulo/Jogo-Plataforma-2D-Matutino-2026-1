# Tutorial: Jogo de Plataforma 2D — Menu, Fases, Chefão e Puzzles

Neste tutorial você vai transformar uma fase simples em um jogo completo, com:

- uma **tela inicial** com os botões Jogar e Sair;
- uma tela de **Game Over** com os botões Tentar de novo e Menu;
- **3 fases**, e uma placa no fim de cada uma que leva para a próxima;
- **espinhos** que tiram vida;
- **plataformas que se movem** e carregam o jogador;
- um **puzzle de empurrar caixa** com botão e porta;
- um **chefão**, o Rei Slime, com barra de vida;
- uma **tela de vitória**.

---

## Sumário

0. [Introdução](#0-introdução)
1. [Conceitos básicos](#1-conceitos-básicos)
2. [Mapa do projeto](#2-mapa-do-projeto)
3. [O que já existia no projeto](#3-o-que-já-existia-no-projeto)
4. [Tela inicial (Menu)](#4-tela-inicial-menu)
5. [Game Over](#5-game-over)
6. [Passar de fase](#6-passar-de-fase)
7. [Montando as fases](#7-montando-as-fases)
8. [Espinhos que dão dano](#8-espinhos-que-dão-dano)
9. [Plataforma móvel](#9-plataforma-móvel)
10. [Puzzle de empurrar caixa](#10-puzzle-de-empurrar-caixa)
11. [O chefão (Rei Slime)](#11-o-chefão-rei-slime)
12. [Tela de vitória e teste final](#12-tela-de-vitória-e-teste-final)

---

## 0. Introdução

### Do que você precisa

- **Unity 6000.0.71f1**, a mesma versão do projeto.
- O projeto base aberto no Unity. Ele já tem um jogador que anda, pula e atira, slimes, moedas, corações de vida e uma área que mata quem cai no buraco.

### Como ler este tutorial

As seções 0 a 3 são de leitura: explicam o que já existe. A partir da seção 4, cada parte segue sempre a mesma ordem:

| Ícone | O que é |
|---|---|
| 🎯 **Objetivo** | O que vamos construir nesta parte |
| 🛠️ **No Editor** | O que fazer dentro do Unity, clique por clique |
| 💻 **O código** | O script, explicado em pedaços |
| ✅ **Como testar** | Como conferir se deu certo |

> 💡 **Dica:** salve a cena com frequência (**Ctrl+S**). Faça as partes **em ordem**, porque cada uma usa coisas criadas nas anteriores.

### Controles do jogo

| Ação | Tecla |
|---|---|
| Andar | **A / D** ou **setas** |
| Pular | **Espaço** |
| Atirar | **Enter** ou **clique do mouse** |

---

## 1. Conceitos básicos

Leia esta seção com calma. Todas as outras usam estas palavras.

| Conceito | O que é |
|---|---|
| **Cena** (*Scene*) | Uma "tela" do jogo, como o Menu, a Fase1 ou a Vitória. Fica salva em `Assets/Scenes`. |
| **GameObject** | Qualquer coisa que existe na cena: o jogador, uma moeda, a câmera, um botão. |
| **Componente** | Uma "peça" que dá um comportamento ao GameObject. Por exemplo, o `SpriteRenderer` desenha uma imagem e o `Rigidbody2D` dá física. Nossos scripts também são componentes. |
| **Script** | Um arquivo `.cs` com uma classe que herda de `MonoBehaviour`. **O nome do arquivo tem que ser igual ao nome da classe.** |
| **`Start()`** | Método que o Unity chama **uma vez**, quando o objeto aparece na cena. |
| **`Update()`** | Método que o Unity chama **a cada frame**, várias vezes por segundo. |
| **Inspector** | Janela que mostra os componentes do objeto selecionado. Toda variável `public` de um script aparece ali e pode ser alterada sem mexer no código. |
| **Tag** | Um "rótulo" para identificar objetos no código, por exemplo `Player`, `Enemy`, `Coin` e `Box`. |
| **Layer** | Uma "camada" de objetos. Neste projeto, tudo que é **chão** fica na layer `Ground`. |
| **Collider 2D** | A "forma física" do objeto (caixa, círculo, cápsula). Faz as coisas baterem umas nas outras. |
| **Trigger** | Um Collider com **Is Trigger** marcado. Ele **não bloqueia** nada, só **avisa** quando algo entra (`OnTriggerEnter2D`) ou sai (`OnTriggerExit2D`). |
| **Collision** | Batida entre dois Colliders normais, os que não são Trigger. Avisa pelo `OnCollisionEnter2D`. |
| **Rigidbody2D** | Componente que coloca o objeto na física: gravidade, velocidade e empurrões. |
| **Prefab** | Um "molde" de GameObject salvo em `Assets/Prefabs`. Você arrasta o molde para a cena quantas vezes quiser. |
| **`Time.deltaTime`** | Quanto tempo, em segundos, passou desde o último frame. Multiplicar por ele faz o jogo rodar na mesma velocidade em qualquer computador. |
| **`Time.timeScale`** | A velocidade do tempo do jogo: `1` é normal e `0` é pausado. |

---

## 2. Mapa do projeto

### As pastas

```
Assets/
├── Scenes/      → as cenas: Menu, Fase1, Fase2, Fase3, Vitoria
├── Scripts/     → todo o código C#
├── Prefabs/     → os moldes: slimes, moeda, tiro, espinho, caixa, plataforma
├── Palettes/    → a paleta de tiles "Season", usada para desenhar as fases
├── Sprite/      → as imagens (personagem, slimes, tileset)
├── Fonts/       → a fonte Kablammo, usada nos títulos
└── Animations/  → as animações do jogador e da moeda
```

### O caminho do jogador pelo jogo

```
                ┌──────── perdeu todas as vidas ────────┐
                ▼                                        │
   [Menu] ──Jogar──► [Fase1] ──placa──► [Fase2] ──placa──► [Fase3] ──venceu o chefão──► [Vitoria]
     ▲                  │                  │                  │                              │
     │                  └──────────── Game Over: "Tentar de novo" recarrega a fase ─────────┘
     │                                                                                       │
     └────────────────────────────────────── botão "Menu" ◄──────────────────────────────────┘
```

### Os scripts

| Script | Onde fica | O que faz |
|---|---|---|
| `PlayerController` | Player | Anda, pula, atira, leva dano |
| `GameManager` | GameManager | Moedas, vidas, HUD, Game Over |
| `SlimeEnemy` | Slimes | Patrulha e machuca o jogador |
| `PlayerProjectile` | Tiro | Voa e causa dano |
| `DeathZone` | Área embaixo da fase | Mata quem cai no buraco |
| `CameraFollow` | Main Camera | Segue o jogador |
| `MenuButtons` | Canvas das telas | Ações dos botões |
| `LevelExit` | Placa de saída | Carrega a próxima fase |
| `Spike` | Espinhos | Tira vida |
| `MovingPlatform` | Plataforma móvel | Vai e volta e carrega o jogador |
| `PressurePlate` | Botão do puzzle | Abre a porta quando tem uma caixa em cima |
| `BossSlime` | Chefão | Patrulha, pula e fica bravo |

---

## 3. O que já existia no projeto

Antes de criar coisas novas, entenda o que já funciona. Abra os scripts em `Assets/Scripts`: todos estão comentados linha a linha.

### `PlayerController`: o jogador

- A cada frame, o `Update()` faz quatro coisas, nesta ordem:
  1. `Gravity()` puxa o jogador para baixo quando ele está no ar.
  2. `Move()` lê as teclas e calcula a velocidade.
  3. `DoAtk()` cria o tiro quando você aperta o ataque.
  4. `UpdateAnimation()` escolhe a animação.
- Por último, o `Update()` aplica a velocidade no `Rigidbody2D`.
- **Como o jogador sabe que está no chão?** O Player tem um filho chamado **Foot**, um pequeno Trigger nos pés. Quando o Foot encosta em algo da **layer `Ground`**, a variável `isGrounded` vira `true`. **Por isso, tudo em que o jogador pode pisar precisa estar na layer `Ground`.**
- `TakeDamage()` tira vida e deixa o jogador invencível por 1 segundo. `DoKnockBack()` empurra o jogador para longe.

### `GameManager`: o juiz da fase

- Guarda as **moedas** (`CollectedCoins`) e as **vidas** (`PlayerLives`).
- Quando o jogador morre, `PlayerDied()` tira uma vida. Se ainda sobrar vida, o jogador renasce no ponto inicial; se não, é **Game Over**.

### `SlimeEnemy`: o inimigo

- Anda de um lado para o outro até a distância `patrolDistance`.
- Se **encostar** no jogador (`OnCollisionEnter2D`), machuca e empurra.
- Se o **pé** do jogador encostar nele (`OnTriggerEnter2D` com a tag `PlayerFoot`), o slime leva dano e o jogador quica.

### `PlayerProjectile`, `DeathZone` e `CameraFollow`

- **PlayerProjectile:** o tiro voa reto e, quando acerta algo com a tag `Enemy`, causa dano.
- **DeathZone:** um Trigger comprido embaixo da fase. Quem cai no buraco encosta nele e morre.
- **CameraFollow:** coloca a câmera na posição do jogador mais um `offset`.

---

## 4. Tela inicial (Menu)

### 🎯 Objetivo

Criar uma cena **Menu** com o título do jogo e os botões **Jogar** e **Sair**.

### 🛠️ No Editor

**Passo 1: renomear a fase atual**

1. Na janela **Project**, abra `Assets/Scenes`.
2. Clique com o botão direito em `SampleScene` → **Rename** → digite `Fase1`.

**Passo 2: remover o painel de início antigo**

1. Abra a cena `Fase1` (dois cliques).
2. Na **Hierarchy**, abra o `Canvas` e apague o objeto `PnlStart` (selecione e aperte **Delete**). Agora quem faz o papel de "tela inicial" é a cena Menu.

**Passo 3: criar o script `MenuButtons`**

1. Em `Assets/Scripts`, clique com o botão direito → **Create → MonoBehaviour Script**. Em algumas versões o caminho é **Create → Scripting → MonoBehaviour Script**.
2. Dê o nome `MenuButtons` e cole o código da seção 💻 abaixo.

**Passo 4: criar a cena Menu**

1. Menu **File → New Scene**, escolha **Lit 2D (URP)** e clique em **Create**.
2. Salve com **Ctrl+S** em `Assets/Scenes` com o nome `Menu`.
3. Selecione a **Main Camera** e mude a cor de **Background** para a cor de fundo que preferir.

**Passo 5: criar o Canvas**

1. Na Hierarchy, clique com o botão direito → **UI → Canvas**. O Unity cria também um **EventSystem**, que é o objeto que faz os cliques funcionarem.
2. Selecione o **EventSystem**. Se aparecer o aviso sobre o *Input System* e o botão **Replace with InputSystemUIInputModule**, clique nele. Sem isso os botões não respondem.
3. Selecione o **Canvas** e, no componente **Canvas Scaler**, configure:
   - **UI Scale Mode:** `Scale With Screen Size`
   - **Reference Resolution:** `1920 x 1080`
   - **Match:** `0.5`

   Assim a interface fica do mesmo tamanho em qualquer resolução.

**Passo 6: o título**

1. Clique com o botão direito no Canvas → **UI → Text - TextMeshPro**. Se o Unity pedir para importar o *TMP Essentials*, clique em **Import**.
2. Renomeie para `txtTitle` e configure:
   - **Pos Y:** `250`; **Width:** `1600`; **Height:** `250`
   - **Text:** o nome do seu jogo, por exemplo `AVENTURA SLIME`
   - **Font Asset:** `Kablammo-Regular-VariableFont_MORF SDF`
   - **Font Size:** `140`
   - **Alignment:** centralizado; escolha a cor que quiser

**Passo 7: os botões**

1. Clique com o botão direito no Canvas → **UI → Button - TextMeshPro**. Renomeie para `BtnJogar`.
2. Configure **Width** `420`, **Height** `110` e **Pos Y** `-20`.
3. Abra o botão, selecione o filho `Text (TMP)` e mude o texto para `Jogar`, com a fonte Kablammo e tamanho `56`.
4. Duplique o botão com **Ctrl+D**, renomeie para `BtnSair`, coloque **Pos Y** `-170` e mude o texto para `Sair`.

**Passo 8: ligar os botões ao código**

1. Selecione o **Canvas** → **Add Component** → `MenuButtons`.
2. Selecione o `BtnJogar`. No componente **Button**, na lista **On Click ()**, clique no **+**.
3. Arraste o **Canvas** da Hierarchy para o campo que diz *None (Object)*.
4. No menu ao lado, que diz *No Function*, escolha **MenuButtons → PlayGame ()**.
5. Repita no `BtnSair`, escolhendo **MenuButtons → QuitGame ()**.

**Passo 9: colocar as cenas no Build**

O Unity só consegue carregar por código as cenas que estão na lista do Build.

1. Menu **File → Build Profiles**. Em versões mais antigas é **File → Build Settings**.
2. Na **Scene List**, arraste as cenas `Menu` e `Fase1` da janela Project para a lista.
3. Deixe o **Menu em primeiro**, porque a primeira cena da lista é a que abre quando o jogo começa.

### 💻 O código: `MenuButtons.cs`

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Fase1");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Debug.LogWarning("Sair do Jogo!");
        Application.Quit();
    }
}
```

**Explicando por partes:**

- `using UnityEngine.SceneManagement;` libera o uso do `SceneManager`, a ferramenta do Unity que **troca de cena**.
- `SceneManager.LoadScene("Fase1")` fecha a cena atual e abre a cena chamada `Fase1`. **O nome precisa ser exatamente igual**, com as mesmas letras maiúsculas e minúsculas.
- Os métodos são `public` para poderem aparecer na lista **On Click ()** do botão.
- `GoToMenu()` ainda não é usado aqui. Ele vai servir para os botões **Menu** das próximas telas.
- `Application.Quit()` fecha o jogo. **Dentro do Editor ele não faz nada**, por isso o `Debug.LogWarning` mostra uma mensagem no Console para você saber que o botão funcionou.

### ✅ Como testar

1. Abra a cena **Menu** e aperte **Play**.
2. Clique em **Jogar**: a `Fase1` deve abrir.
3. Volte para o Menu e clique em **Sair**: deve aparecer "Sair do Jogo!" no **Console**.

> ⚠️ Se a Fase1 abrir **parada**, sem nada se mexer, é porque o `GameManager` ainda pausa o jogo no começo. Isso é corrigido na próxima seção.

---

## 5. Game Over

### 🎯 Objetivo

Quando o jogador perde todas as vidas, aparece o painel **GAME OVER** com dois botões:

- **Tentar de novo:** recomeça a mesma fase;
- **Menu:** volta para a tela inicial.

### 🛠️ No Editor

**Passo 1: ajustar o `GameManager`.** Faça as alterações mostradas na seção 💻 abaixo.

**Passo 2: arrumar o painel**

1. Abra a `Fase1`. No `Canvas`, ative o `PnlGameOver` só para enxergar (marque a caixinha ao lado do nome no Inspector).
2. Renomeie o texto `Text (TMP)` do painel para `txtGameOver`. Coloque a fonte Kablammo e a cor vermelha.
3. Renomeie o botão `Button` para `BtnRestart`. Coloque **Width** `420` e mude o texto para `Tentar de novo`, com tamanho `48`. A lista **On Click** dele já chama `GameManager.Restart`.
4. Duplique o botão com **Ctrl+D**, renomeie para `BtnMenu`, coloque **Pos Y** `-130` e mude o texto para `Menu`.

**Passo 3: ligar o botão Menu**

1. Selecione o **Canvas** → **Add Component** → `MenuButtons`.
2. No `BtnMenu`, na lista **On Click**, troque a função de `GameManager.Restart` para **MenuButtons → GoToMenu ()**. Para isso, arraste o **Canvas** para o campo do objeto.
3. **Desative o `PnlGameOver` de novo**, porque ele só deve aparecer no Game Over.

### 💻 O código: mudanças no `GameManager.cs`

**1) Remova** a variável `PnlStart` e o método `StartGame()` inteiro, que não são mais usados.

**2) No `Start()`**, troque `Time.timeScale = 0;` por:

```csharp
void Start()
{
    //Garante que o jogo não comece pausado (o Game Over pausa o tempo)
    Time.timeScale = 1;
    player = FindFirstObjectByType<PlayerController>();
}
```

**3) No `Restart()`**, troque `SceneManager.LoadScene(0);` por:

```csharp
public void Restart()
{
    //Recarrega a fase atual
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
```

**Explicando por partes:**

- **Por que `Time.timeScale = 1` no Start?** O `GameOver()` coloca `Time.timeScale = 0` para congelar o jogo. Quando você clica em "Tentar de novo" ou "Menu", a nova cena abre **com o tempo ainda congelado**. Sem esta linha a fase abriria parada. Colocar `1` no começo de toda fase resolve.
- **Por que `GetActiveScene().name`?** Antes, `LoadScene(0)` sempre carregava a primeira cena da lista, que agora é o Menu. `GetActiveScene().name` devolve o nome da **cena que está aberta agora**. Assim, quem morre na Fase2 recomeça na Fase2.
- O resto do Game Over já existia. `PlayerDied()` tira uma vida e, quando `PlayerLives` chega a 0, chama `GameOver()`, que liga o painel com `PnlGameOver.SetActive(true)`.

### ✅ Como testar

1. Na `Fase1`, selecione o `GameManager` e coloque **Player Lives** = `1`, só para o teste.
2. Aperte **Play** e caia em um buraco: o painel GAME OVER deve aparecer.
3. Clique em **Tentar de novo**: a fase recomeça **se mexendo**.
4. Morra de novo e clique em **Menu**: você volta para a tela inicial.
5. **Volte o Player Lives para `3`.**

---

## 6. Passar de fase

### 🎯 Objetivo

Colocar uma **placa** no fim da fase. Quando o jogador encosta nela, a próxima fase abre.

### 🛠️ No Editor

**Passo 1:** crie o script `LevelExit` com o código da seção 💻.

**Passo 2: criar a placa**

1. Na `Fase1`, clique com o botão direito na Hierarchy → **Create Empty**. Renomeie para `Saida` e coloque no fim da fase, em cima do chão.
2. Coloque **Scale** `2, 2, 1`.
3. **Add Component → Sprite Renderer**. No campo **Sprite**, clique na bolinha e escolha `Season_collection_100`, que é a placa de madeira.
4. **Add Component → Box Collider 2D**. Configure:
   - **Is Trigger** ✔
   - **Size** `1, 3`
   - **Offset** `0, 1`

   O colisor fica alto de propósito, para o jogador não conseguir pular por cima da placa.
5. **Add Component → Level Exit**. No campo **Next Level**, digite `Fase2`.

**Passo 3: criar as outras fases**

1. Salve a `Fase1`.
2. Na janela Project, selecione `Fase1` e aperte **Ctrl+D** duas vezes. Renomeie as cópias para `Fase2` e `Fase3`.
3. Abra a `Fase2` e mude o **Next Level** da placa para `Fase3`.
4. Na `Fase3`, **apague a placa**: lá a fase termina quando o chefão é derrotado.
5. Coloque `Fase2` e `Fase3` no **Build Profiles**, nesta ordem: `Menu`, `Fase1`, `Fase2`, `Fase3`.

### 💻 O código: `LevelExit.cs`

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public string NextLevel;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            SceneManager.LoadScene(NextLevel);
        }
    }
}
```

**Explicando por partes:**

- `public string NextLevel;` é o nome da próxima cena. Como é `public`, ele aparece no Inspector, e **o mesmo script serve para todas as fases**: só o texto digitado muda.
- `OnTriggerEnter2D` é chamado sozinho pelo Unity quando algo entra no Trigger da placa.
- `collision.tag == "Player"` garante que só o **jogador** troca de fase. Um slime ou um tiro encostando na placa não faz nada.

### ✅ Como testar

Aperte **Play** na `Fase1`, ande até a placa e confira se a `Fase2` abre.

---

## 7. Montando as fases

### 🎯 Objetivo

Desenhar o chão das fases com a **Tile Palette** e espalhar inimigos, moedas e obstáculos.

### 🛠️ No Editor

**Passo 1: abrir a paleta**

1. Menu **Window → 2D → Tile Palette**.
2. No topo da janela, escolha a paleta **Season**. Os tiles aparecem organizados do mesmo jeito que estão na imagem `Season_collection.png`.

**Passo 2: entender os Tilemaps da cena**

Dentro do objeto `Grid` da cena existem dois Tilemaps:

| Tilemap | Para que serve | Tem colisão? |
|---|---|---|
| `Floor` | O chão, onde o jogador pisa | ✔ Sim, e fica na layer **Ground** |
| `Background` | Árvores e arbustos de enfeite | ✘ Não |

Na Tile Palette, o campo **Active Tilemap** escolhe em qual Tilemap você vai desenhar. **Para o chão, escolha `Floor`.**

**Passo 3: desenhar**

| Ferramenta | Atalho | Uso |
|---|---|---|
| Pincel | **B** | Pinta um tile por vez |
| Retângulo | **U** | Preenche uma área inteira |
| Borracha | **D** | Apaga |
| Conta-gotas | **I** | Copia um tile da cena |

Monte cada bloco de chão assim:

- na **linha de cima**, os tiles com grama, com cantos arredondados nas pontas;
- nas **laterais**, os tiles de borda;
- no **meio**, os tiles de terra.

**Passo 4: usar um tema diferente em cada fase**

O tileset tem quatro estações. Usamos uma por fase:

| Fase | Tema |
|---|---|
| Fase1 | 🌳 Verde |
| Fase2 | 🍂 Outono |
| Fase3 | ❄️ Neve |

Os tiles de cada tema ficam em lugares equivalentes da paleta.

**Passo 5: as medidas do pulo**

O jogador pula **pouco mais de 1 bloco de altura** e alcança **uns 5 blocos de distância**. Desenhe pensando nisso:

| Obstáculo | Tamanho seguro |
|---|---|
| Degrau para subir | **1 bloco** (2 blocos o jogador **não** sobe sozinho) |
| Buraco | **2 a 3 blocos** de largura |
| Parede que bloqueia | **2 blocos ou mais** de altura |

**Passo 6: colocar objetos**

Arraste prefabs da pasta `Assets/Prefabs` para a cena:

| Prefab | O que é |
|---|---|
| `GreenSlime` | Slime de 1 vida |
| `BlueSlime` | Slime de 5 vidas |
| `Coin` | Moeda |
| `Spike` | Espinho (seção 8) |
| `Box` | Caixa empurrável (seção 10) |
| `MovingPlatform` | Plataforma móvel (seção 9) |

**Passo 7: ajustar a DeathZone**

Selecione o objeto `DeathZone` e aumente o **Size X** do **Box Collider 2D** até ele ficar embaixo de **todos** os buracos da fase.

**Passo 8: colocar paredes nas pontas**

Desenhe uma coluna alta de chão no começo da fase, para o jogador não cair pela esquerda. Faça o mesmo no fim da Fase3, para fechar a arena do chefão.

### ✅ Como testar

Jogue a fase inteira. Confira se:

- todos os pulos são possíveis;
- cair em qualquer buraco tira uma vida;
- a placa leva para a próxima fase.

---

## 8. Espinhos que dão dano

### 🎯 Objetivo

Hoje o espinho usa o script `DeathZone` e **mata na hora**. Vamos criar um espinho que **tira 1 coração e empurra** o jogador, igual quando um slime encosta nele.

### 🛠️ No Editor

**Passo 1:** crie o script `Spike` com o código da seção 💻.

**Passo 2: criar o prefab do espinho**

1. Abra a `Fase1` e selecione o espinho que já existe, o objeto `itens_10`.
2. No Inspector, remova o componente **Death Zone**: clique nos três pontinhos ⋮ → **Remove Component**.
3. **Add Component → Spike**.
4. Confira se o **Box Collider 2D** está com **Is Trigger** ✔.
5. Renomeie o objeto para `Spike` e **arraste-o para a pasta `Assets/Prefabs`**. Pronto, virou um prefab.

**Passo 3:** espalhe espinhos pelas fases arrastando o prefab `Spike`. Uma boa ideia é colocar moedas em cima deles, para o jogador ter que pular.

### 💻 O código: `Spike.cs`

```csharp
using UnityEngine;

public class Spike : MonoBehaviour
{
    public int Damage = 1;
    public float KnockbackForce = 4;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if(player == null)
                return;

            //Empurra o jogador para longe do espinho e tira vida
            Vector3 knockBackDir = collision.transform.position - transform.position;
            player.DoKnockBack(knockBackDir, KnockbackForce);
            player.TakeDamage(Damage);
        }
    }
}
```

**Explicando por partes:**

- `collision.tag == "Player"` faz o espinho reagir só ao **corpo** do jogador. O pé dele tem outra tag, `PlayerFoot`, e é ignorado.
- `GetComponent<PlayerController>()` pega o script do jogador para podermos chamar os métodos dele.
- `if(player == null) return;` é uma proteção: se por algum motivo não achar o script, sai do método sem dar erro.
- **A direção do empurrão** é `posição do jogador - posição do espinho`. Essa conta dá uma seta que **sai do espinho e aponta para o jogador**, então o jogador é jogado para longe do espinho.
- `DoKnockBack` e `TakeDamage` já existiam no `PlayerController`. **Reaproveitamos** o mesmo comportamento do slime.
- **E se o jogador ficar em cima do espinho?** O `TakeDamage` ignora dano enquanto o jogador está invencível, que dura 1 segundo. E o empurrão tira ele de cima.

### ✅ Como testar

Pule em cima de um espinho. Você deve ser empurrado e perder **1 coração**, não uma vida inteira. Encostando em espinhos 3 vezes, aí sim você perde uma vida e volta para o começo.

---

## 9. Plataforma móvel

### 🎯 Objetivo

Criar uma plataforma que **vai e volta** sobre um buraco e **carrega o jogador** em cima dela.

### 🛠️ No Editor

**Passo 1:** crie o script `MovingPlatform` com o código da seção 💻.

**Passo 2: criar a plataforma**

1. **Create Empty** e renomeie para `MovingPlatform`.
2. No topo do Inspector, mude a **Layer** para `Ground`. **Isto é muito importante:** é assim que o pé do jogador sabe que está pisando em chão e pode pular de novo.
3. **Add Component → Box Collider 2D** com **Size** `3, 1`. O **Is Trigger** fica **desmarcado**, porque a plataforma tem que ser sólida.
4. **Add Component → Moving Platform**.

**Passo 3: o desenho da plataforma**

1. Clique com o botão direito na plataforma → **Create Empty** três vezes, criando os filhos `Tile0`, `Tile1` e `Tile2`.
2. Coloque as posições X em `-1`, `0` e `1`.
3. Em cada um, **Add Component → Sprite Renderer** e escolha os sprites do topo de grama:
   - `Tile0`: canto esquerdo, `Season_collection_67`
   - `Tile1`: meio, `Season_collection_69`
   - `Tile2`: canto direito, `Season_collection_70`

**Passo 4: salvar e usar**

1. Arraste a plataforma para `Assets/Prefabs` para virar prefab.
2. Coloque uma plataforma sobre um buraco largo.
3. No Inspector, ajuste:
   - **Distance:** quanto ela anda. `4, 0, 0` anda 4 blocos para a direita. Use `0, 3, 0` para uma plataforma que **sobe e desce**.
   - **Speed:** a velocidade, em blocos por segundo.

### 💻 O código: `MovingPlatform.cs`

```csharp
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 Distance = new Vector3(4, 0, 0);
    public float Speed = 2;

    Vector3 startPosition;
    Vector3 endPosition;
    bool goingToEnd = true;

    void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + Distance;
    }

    void Update()
    {
        Vector3 target = endPosition;
        if(goingToEnd == false)
        {
            target = startPosition;
        }

        transform.position = Vector3.MoveTowards(transform.position, target, Speed * Time.deltaTime);

        //Chegou no destino: volta pelo outro lado
        if(transform.position == target)
        {
            goingToEnd = !goingToEnd;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && collision.transform.position.y > transform.position.y)
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
```

**Explicando por partes:**

**Parte 1: os dois pontos (`Start`)**

- `startPosition` é onde a plataforma foi colocada na cena.
- `endPosition = startPosition + Distance` é o ponto de chegada. Somar vetores é somar cada eixo: `(10, -3, 0) + (4, 0, 0) = (14, -3, 0)`.

**Parte 2: o movimento (`Update`)**

- `goingToEnd` diz se a plataforma está **indo** (`true`) ou **voltando** (`false`). Com base nele escolhemos o `target`, o destino.
- `Vector3.MoveTowards(onde estou, para onde vou, passo máximo)` devolve uma posição **um pouquinho mais perto** do destino, sem nunca passar dele.
- O passo é `Speed * Time.deltaTime`. Em 1 segundo, a soma de todos os passos dá exatamente `Speed` blocos.
- Quando `transform.position == target`, a plataforma chegou. Então `goingToEnd = !goingToEnd` inverte o valor: o `!` significa "o contrário de", então `true` vira `false` e `false` vira `true`.

**Parte 3: carregar o jogador (`OnCollisionEnter2D` / `OnCollisionExit2D`)**

- **O problema:** a plataforma anda, mas o jogador fica parado no mesmo lugar e escorrega para fora dela.
- **A solução:** `SetParent(transform)` transforma o jogador em **filho** da plataforma. No Unity, quando o **pai** se move, os **filhos** se movem junto.
- `collision.transform.position.y > transform.position.y` confere se o jogador está **em cima** da plataforma. Se ele bater por baixo ou pelo lado, não vira filho.
- Quando o jogador pula ou sai andando, `OnCollisionExit2D` chama `SetParent(null)`, e ele volta a ser independente.

### ✅ Como testar

1. Aperte **Play** e suba na plataforma: ela deve te levar junto, sem você escorregar.
2. Pule de cima dela: o pulo deve funcionar normalmente.
3. Se você **não consegue pular** em cima da plataforma, confira se a **Layer** dela é `Ground`.

---

## 10. Puzzle de empurrar caixa

### 🎯 Objetivo

Criar uma **caixa** que o jogador empurra andando contra ela. Vamos usar a caixa em dois puzzles:

- **Puzzle A (Fase1):** empurrar a caixa até um muro alto e usá-la de **degrau**.
- **Puzzle B (Fase2):** empurrar a caixa para cima de um **botão** que abre uma **porta**.

### 🛠️ No Editor: a caixa

**Passo 1: criar a tag `Box`**

1. Selecione qualquer objeto. No Inspector, abra o menu **Tag** → **Add Tag...**.
2. Clique no **+**, digite `Box` e clique em **Save**.

**Passo 2: criar o prefab da caixa**

A `Fase1` já tem um bloco de pedra, o `Season_collection_37`, mas ele é pesado demais (massa 50) e quase não se move.

1. Duplique o bloco com **Ctrl+D** e renomeie a cópia para `Box`.
2. **Tag:** `Box`.
3. No **Rigidbody 2D**:
   - **Mass:** `2`. Leve o bastante para o jogador, que tem massa 1, conseguir empurrar.
   - **Constraints → Freeze Rotation Z** ✔. Assim a caixa não tomba nem rola.
4. No **Box Collider 2D**, coloque **Size** `0.95, 1`. Um pouquinho mais estreita, para ela não enganchar nas emendas do chão nem nas paredes.
5. A caixa tem um filho chamado `Floor`, que está na layer **Ground**. Ele é o "chão" em cima da caixa e permite que o jogador **suba e pule** dela. Coloque o **Size X** dele em `0.95` também.
6. Arraste a `Box` para `Assets/Prefabs`.

### 🛠️ No Editor: Puzzle A, o degrau (Fase1)

Este puzzle **não precisa de código novo**.

1. No fim da Fase1, desenhe um **muro de 2 blocos** de altura antes da placa. Ele é alto demais para o pulo.
2. Coloque uma `Box` alguns blocos antes do muro.
3. O jogador empurra a caixa até o muro, sobe nela (1 bloco) e dali pula para o topo do muro (mais 1 bloco).
4. **Cuidado com o lado oposto:** se tiver um buraco atrás da caixa, o jogador pode empurrá-la para dentro dele por engano, e aí a fase fica **impossível de terminar**. Para evitar isso, coloque um **degrauzinho de 1 bloco** antes da caixa. Ele segura a caixa, e o jogador consegue pular por cima dele.

### 🛠️ No Editor: Puzzle B, botão e porta (Fase2)

**Passo 1:** crie o script `PressurePlate` com o código da seção 💻.

**Passo 2: o poço.** No fim da Fase2, desenhe um **poço de 2 blocos de largura e 1 bloco de fundura** no chão.

> 🐞 **Lição de level design:** na primeira versão, o poço tinha só **1 bloco** de largura. A caixa, empurrada rápido, **passava por cima do poço sem cair** e parava encostada na porta, e o jogador ficava preso. Com **2 blocos**, a caixa sempre cai, e a parede do poço segura ela lá dentro. **Sempre teste os seus puzzles!**

**Passo 3: a porta**

1. **Create Empty** e renomeie para `Porta`. Coloque logo depois do poço, em cima do chão.
2. **Layer:** `Ground`.
3. **Add Component → Box Collider 2D** com **Size** `1, 3` e **Offset** `0, 1.5`. Ela fica com 3 blocos de altura, alta demais para pular por cima.
4. Crie 3 filhos (`Bloco0`, `Bloco1`, `Bloco2`) com **Sprite Renderer** usando o sprite `Season_collection_89`, o bloco roxo. Coloque as posições Y em `0.5`, `1.5` e `2.5`.

**Passo 4: o botão**

1. **Create Empty** e renomeie para `Botao`. Coloque no **fundo do poço**.
2. **Scale:** `1.6, 0.25, 1`, para ficar achatado como um botão.
3. **Add Component → Sprite Renderer** com o sprite `Season_collection_116`, o bloco verde.
4. **Add Component → Box Collider 2D** com **Is Trigger** ✔ e **Size** `1, 2`.
5. **Add Component → Pressure Plate** e **arraste a `Porta` da Hierarchy para o campo `Door`**.

**Passo 5:** coloque uma `Box` antes do poço, com um degrauzinho atrás dela, pelo mesmo motivo do Puzzle A.

### 💻 O código: `PressurePlate.cs`

```csharp
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject Door;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Box")
        {
            Door.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Box")
        {
            Door.SetActive(true);
        }
    }
}
```

**Explicando por partes:**

- `public GameObject Door;` é o campo onde você arrastou a porta no Inspector. Assim o botão sabe **qual** porta controlar, e você pode ter vários botões com portas diferentes.
- `OnTriggerEnter2D` roda quando algo **entra** no botão. Se for uma caixa (`tag == "Box"`), `Door.SetActive(false)` **desliga** a porta: ela some e deixa de bloquear.
- `OnTriggerExit2D` roda quando algo **sai** do botão. Se a caixa sair, `SetActive(true)` **liga** a porta de novo.
- **Por que verificar a tag?** Sem o `if`, o próprio jogador pisando no botão abriria a porta, e não existiria puzzle.

### ✅ Como testar

- **Puzzle A:** empurre a caixa até o muro, suba nela e depois no muro.
- **Puzzle B:** pise sozinho no botão. A porta **não** abre. Empurre a caixa para dentro do poço: a porta **some**.

---

## 11. O chefão (Rei Slime)

### 🎯 Objetivo

No fim da Fase3, criar um **slime gigante** que:

- anda de um lado para o outro e **pula** de tempos em tempos;
- tem **10 de vida**, mostrada em uma **barra no topo da tela**;
- com metade da vida, fica **vermelho e mais rápido**;
- quando morre, abre a **tela de vitória**.

### 🛠️ No Editor

**Passo 1:** crie o script `BossSlime` com o código da seção 💻.

**Passo 2: a arena.** No fim da Fase3, deixe um trecho **plano e comprido** (uns 25 blocos), com uma **parede alta** no final.

**Passo 3: criar o chefão**

1. Arraste o prefab `GreenSlime` para a arena.
2. Clique com o botão direito nele → **Prefab → Unpack Completely**. Assim ele vira um objeto comum e as mudanças não afetam os outros slimes.
3. Renomeie para `BossSlime`.
4. **Remova** o componente **Slime Enemy** (⋮ → **Remove Component**) e **Add Component → Boss Slime**.
5. **Scale:** `3, 3, 1`.
6. No **Sprite Renderer**, mude a **Color** para roxo.
7. Confira se a **Tag** é `Enemy`, porque é ela que faz o tiro do jogador acertar o chefão.

**Passo 4: a barra de vida**

1. No `Canvas`, crie **Create Empty** e renomeie para `BossHUD`.
2. Nas **Anchor Presets**, o quadradinho no canto do RectTransform, escolha **top-center**. Coloque **Pos Y** `-20`, **Width** `500` e **Height** `90`.
3. Dentro do `BossHUD`, crie **UI → Text - TextMeshPro** com o texto `REI SLIME`, posicionado no alto.
4. Dentro do `BossHUD`, crie **UI → Image** e renomeie para `imgBossLifeBg`. Ela é o fundo escuro da barra: faça uma faixa de `500 x 36` na parte de baixo, com cor preta semitransparente.
5. **Dentro do `imgBossLifeBg`**, crie outra **UI → Image** e renomeie para `imgBossLife`. Configure:
   - **Source Image:** `UISprite`. Clique na bolinha e procure por esse nome.
   - **Color:** vermelho
   - **Image Type:** `Filled`; **Fill Method:** `Horizontal`; **Fill Amount:** `1`
   - Estique a imagem para ocupar todo o fundo: em **Anchor Presets**, use **stretch-stretch** segurando **Alt**.
6. Selecione o `BossSlime` e **arraste o `imgBossLife` para o campo `Img Boss Life`**.
7. Na Hierarchy, arraste o `PnlGameOver` para ser o **último filho** do Canvas. O que está mais embaixo na lista é desenhado **por cima**, e o Game Over precisa cobrir a barra do chefão.

**Passo 5: fazer o tiro acertar o chefão.** Altere o `PlayerProjectile.cs` como mostra a seção 💻.

### 💻 O código: `BossSlime.cs`

O script completo está em `Assets/Scripts/BossSlime.cs`. Vamos por partes.

**Parte 1: as variáveis**

```csharp
[Header("Health")]
public int MaxHealth = 10;
public Image imgBossLife;
int currentHealth;

[Header("Movement")]
public float moveSpeed = 2;
public float angrySpeed = 4;
public float patrolDistance = 5;
int facingDirection = 1;
Vector2 startPosition;

[Header("Jump")]
public float JumpForce = 8;
public float JumpInterval = 3;
float jumpTimer;
```

- `[Header("...")]` só organiza o Inspector em grupos com título.
- `imgBossLife` é do tipo `Image`, por isso o script precisa de `using UnityEngine.UI;` lá no topo.
- `angrySpeed` é a velocidade quando ele fica bravo.
- `jumpTimer` é um **cronômetro** que conta o tempo até o próximo pulo.

**Parte 2: andar (`Move`)**

```csharp
void Move()
{
    if(transform.position.x > startPosition.x + patrolDistance)
    {
        facingDirection = -1;
    }else if(transform.position.x < startPosition.x - patrolDistance)
    {
        facingDirection = 1;
    }

    spriteRenderer.flipX = facingDirection == -1;

    rb.linearVelocity = new Vector2(facingDirection * moveSpeed, rb.linearVelocityY);
}
```

- A patrulha é **igual à do slime comum**: passou do limite, troca de direção.
- `spriteRenderer.flipX = facingDirection == -1;` espelha o desenho quando ele anda para a esquerda. A comparação `==` já dá `true` ou `false`.
- **Atenção ao `rb.linearVelocityY`:** mudamos só a velocidade **X** e mantemos a **Y** que a física calculou. Se colocássemos `0` no Y, como faz o slime comum, o chefão **nunca cairia depois de pular**.

**Parte 3: pular (`Jump`), um cronômetro**

```csharp
void Jump()
{
    jumpTimer -= Time.deltaTime;
    if(jumpTimer <= 0)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocityX, JumpForce);
        jumpTimer = JumpInterval;
    }
}
```

- A cada frame, o cronômetro **diminui** o tempo que passou.
- Quando chega a 0, o chefão pula (velocidade Y = `JumpForce`) e o cronômetro **recomeça** de `JumpInterval`.
- O jogador usa esse mesmo padrão de cronômetro na invencibilidade e no tempo entre tiros.

**Parte 4: levar dano (`TakeDamage`)**

```csharp
public void TakeDamage(int damage)
{
    currentHealth -= damage;
    imgBossLife.fillAmount = (float)currentHealth / MaxHealth;

    if(currentHealth <= MaxHealth / 2)
    {
        moveSpeed = angrySpeed;
        spriteRenderer.color = Color.red;
    }

    if(currentHealth <= 0)
    {
        SceneManager.LoadScene(VictoryScene);
    }
}
```

- `fillAmount` vai de `0` (barra vazia) até `1` (barra cheia). Com 7 de vida sobrando, a conta é `7 / 10 = 0.7`, e a barra fica 70% cheia.
- **Por que `(float)`?** Em C#, **inteiro dividido por inteiro dá inteiro**: `7 / 10` daria `0`! O `(float)` transforma o número em decimal antes da divisão.
- **O modo bravo:** quando a vida chega na metade, troca a velocidade e pinta o chefão de vermelho.
- **A vitória:** com a vida em 0, carrega a cena `Vitoria`.

**Parte 5: encostar e ser pisado.** `OnCollisionEnter2D` e `OnTriggerEnter2D` são **iguais aos do `SlimeEnemy`**. Encostar no corpo dele machuca o jogador, e pisar na cabeça dele causa dano no chefão.

### 💻 O código: mudança no `PlayerProjectile.cs`

Dentro do `if(collision.tag == "Enemy")`, logo depois do bloco do `slimeEnemy`, adicione:

```csharp
//É o chefão?
BossSlime boss = collision.GetComponent<BossSlime>();
if(boss != null)
{
    boss.TakeDamage(1);
}
```

- O tiro **tenta** pegar o script `BossSlime` do objeto atingido. Se o objeto for um slime comum, `boss` fica `null` e nada acontece. Se for o chefão, ele leva dano.

### ✅ Como testar

1. Jogue a Fase3 até a arena.
2. Acerte o chefão com tiros e pulando na cabeça dele. A barra deve diminuir.
3. Com metade da vida, ele deve ficar **vermelho e mais rápido**.
4. Ao zerar a barra, a tela de vitória deve abrir. Ela é criada na próxima seção; até lá, aparece um erro no Console dizendo que a cena `Vitoria` não existe.

---

## 12. Tela de vitória e teste final

### 🎯 Objetivo

Criar a cena **Vitoria** e testar o jogo do começo ao fim.

### 🛠️ No Editor

**Passo 1: criar a cena**

1. Na janela Project, duplique a cena `Menu` com **Ctrl+D** e renomeie para `Vitoria`.
2. Abra a `Vitoria` e configure:
   - Título `VOCÊ VENCEU!`, e se quiser uma cor de fundo diferente na câmera.
   - **Apague** o botão `BtnSair`.
   - Renomeie o `BtnJogar` para `BtnMenu` e mude o texto para `Menu`.
   - Na lista **On Click** do `BtnMenu`, troque a função para **MenuButtons → GoToMenu ()**.

**Passo 2: conferir a lista do Build**

Abra **File → Build Profiles** e confira se a lista está **exatamente nesta ordem**:

```
0  Menu
1  Fase1
2  Fase2
3  Fase3
4  Vitoria
```

### ✅ Teste final: o jogo inteiro

Abra a cena **Menu**, aperte **Play** e marque cada item:

- [ ] O Menu aparece e o botão **Jogar** abre a Fase1
- [ ] Os espinhos tiram **1 coração** e empurram
- [ ] Cair em um buraco tira **1 vida** e o jogador volta para o começo
- [ ] A caixa da Fase1 serve de degrau para subir o muro
- [ ] A placa da Fase1 leva para a Fase2
- [ ] A plataforma móvel da Fase2 carrega o jogador
- [ ] A caixa no poço abre a porta da Fase2
- [ ] A placa da Fase2 leva para a Fase3
- [ ] O chefão perde vida com tiros e pisões, e fica vermelho na metade
- [ ] Derrotar o chefão abre a tela de Vitória
- [ ] O botão **Menu** da Vitória volta para o Menu
- [ ] Perder todas as vidas mostra o **Game Over**
- [ ] **Tentar de novo** recomeça a mesma fase, e ela não fica parada
- [ ] O botão **Menu** do Game Over volta para o Menu

🎉 **Parabéns! Você terminou o seu jogo de plataforma!**
