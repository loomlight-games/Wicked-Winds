> ### Presentación
> Somos Loomlight, un grupo de estudiantes de diseño y desarrollo de videojuegos y creadores de Wicked Winds. Se trata de un proyecto conjunto entre varias asignaturas del curso con el que tratamos de crear un videojuego de navegador de internet que consiga ser completo y divertido, y nos permita acercarnos a la realidad del mercado de los videojuegos.
> ### Miembros
> Alba Haro Ballesteros
> 
> Alfonso del Pino García
> 
> Álvaro Moreno García
> 
> Cristina Lozano Bautista
> 
> Paula González Stradiotto

# Wicked Winds - Documento de Diseño de Juego

<div align="center"><b><i>
    Versión 0.0.5
</i></b></div>

# 1. INTRODUCCIÓN

- **Título**: Wicked Winds
- **Desarrollador**: Loomlight
- **Género**: Juego de estilo **arcade**/**acción** contrarreloj competitivo
- **Estilo artístico**: 3D *lowpoly* y *cartoon* con temática de magos
- **Pilares del diseño**: Inmersión, desafío y emoción
- **Plataforma**: Navegador de Internet
- **Categoría**: Juego rápido de misiones, basado en la acumulación de tiempo, con una mecánica similar a [Crazy Taxi](https://es.wikipedia.org/wiki/Crazy_Taxi), pero ambientado en un mundo de magia.
- **Cámara**: 3D tercera persona
- **Periféricos**:
    - PC: Ratón
    - Dispositivo móvil: Pantalla táctil
- **Controles**: Al ser un juego de navegador y multiplataforma, los controles deben basarse en la interacción con su interfaz mediante *clicks*/*taps*. El movimiento del personaje jugable se controla mediante un *joystick* en pantalla.
- **Puntuación**: el jugador se clasifica en un *ranking* en línea junto a otros jugadores según el mayor tiempo que dure jugando.
- **Entorno**: La partida se produce en un escenario urbano que el jugador puede recorrer libremente.
- **Musica/Sonido**: La música y el diseño sonoro estarán enfocados en generar una atmósfera envolvente, con melodías dinámicas que se adapten al ritmo de la acción y sonidos que refuercen la tensión en momentos clave.

### Descripción breve de la historia y personajes
Ashen, un brujo que vive en una ciudad encantadora y bulliciosa, usa su escoba para cumplir los recados urgentes de los habitantes. Debe completarlos rápidamente para ganar tiempo extra y evitar que el reloj llegue a cero, lo que marcaría el final del juego.

### Descripción breve del concepto
El jugador controla a Ashen mientras se desplaza en su escoba encantada para ayudar a los vecinos. Recolecta objetos que potencian su velocidad y cumple misiones cronometradas, ganando tiempo con cada recado completado y manteniendo el juego en marcha.

# 2. MONETIZACIÓN

## Tipo de Modelo de Monetización
Wicked Winds es un videojuego *freemium*, ya que es gratis pero incluye una tienda donde el jugador puede realizar micropagos para obtener más monedas que le permitan comprar *ítems* y *skins* para personalizar su partida. Cabe destacar que las compras realizadas en la tienda nunca supondrán una ventaja del jugador frente a los demás, ya que las clasificaciones de los jugadores deben ser justas.

Aparte de con micropagos, las monedas de tienda también se pueden conseguir: jugando, ya que se pueden encontrar unas pocas repartidas por el escenario en cada partida; o viendo anuncios, que aunque darán más monedas que jugando, sigue siendo una menor cantidad comparada con las que se pueden conseguir comprándolas directamente con micropagos.

En un futuro se podrían aumentar los beneficios económicos del juego con:
- Implementación de más tipos de monedas/objetos especiales, lo que permitiría justificar más micropagos opcionales.
- Creación y distribución de *merchandising* de Wicked Winds.
- Colaboraciones publicitarias con marcas conocidas que se alineen con la temática del juego.

# 3. PLANIFICACIÓN Y COSTES
## El equipo humano
Nuestro equipo de desarrollo está compuesto por **cinco personas**, cada una con un rol fundamental para llevar el proyecto a cabo de forma eficiente y creativa. Este equipo se divide en dos áreas principales: *programación* y *diseño*, además de la gestión de redes sociales.

### Programación:
**Tres** miembros del equipo se encargan de la programación del videojuego, trabajando en el desarrollo de la lógica del juego, las mecánicas de interacción y la integración técnica de todos los elementos visuales y de jugabilidad. Su labor incluye la creación de sistemas que soporten el flujo de misiones, la gestión de tiempo en partida, la optimización del rendimiento y la resolución de problemas técnicos para asegurar una experiencia fluida y estable.

### Arte y redes sociales:
- **Diseño de personajes y redes sociales:** Uno de los integrantes se ocupa del arte y modelado de los personajes, encargándose de diseñar a Ashen y otros personajes con el estilo visual del juego. También es responsable de la estrategia y gestión de redes sociales, creando y compartiendo contenido para construir y mantener una comunidad de seguidores en torno al proyecto, actualizando a los seguidores sobre el progreso y novedades del juego.

- **Creación de escenarios y mapas:** La segunda integrante en el área de arte se dedica a la creación y modelado de escenarios, mundos y mapas. Su trabajo es fundamental para definir el ambiente visual y los detalles de los entornos en los que Ashen y los habitantes de la ciudad interactúan. Además, se asegura de que cada espacio mantenga la coherencia con la estética y temática del juego, apoyando una experiencia inmersiva. Aunque no es la encargada principal de redes, colabora en el diseño de las publicaciones, apoyando en la creación de gráficos y materiales visuales para lograr una comunicación atractiva y coherente en las plataformas.

## Estimación temporal del desarrollo
La estimación temporal para el desarrollo del videojuego está dividida en varias fases clave, cada una con tareas específicas. Cada fase tiene una duración aproximada, y se contemplan tiempos de prueba y ajustes para asegurar la calidad del producto final.

### 1. Fase de Preproducción : Idea inicial (2-3 semanas)
- **Documentación de Diseño:** Creación del GDD (Game Design Document) con detalles de mecánicas, historia y estética.

- **Conceptos de Arte:** Bocetos iniciales de personajes y escenarios para definir el estilo visual.

- **Planeación Técnica:** Definición de herramientas, tareas e ideas para la parte de desarrollo.

### 2. Fase de Desarrollo: Alpha (10-12 semanas)
- **Programación de Mecánicas Básicas:** Implementación de movimiento, interacción, tienda y sistema de misiones.

- **Diseño y Modelado de Personajes:** Creación y modelado 3D de personajes principales y secundarios.

- **Diseño y Modelado de Escenarios y Mapas:** Creación y modelado 3D de los escenarios y detalles ambientales.

- **Programación de UI/HUD:** Implementación de la interfaz, incluyendo el reloj y las barras de impulso.

- **Integración de Elementos y Ajustes:** Ajuste e integración de personajes, escenarios y mecánicas en un proyecto unificado.

### 3. Fase de Pruebas y Optimización: Beta (2-3 semanas)
- **Pruebas de Jugabilidad:** Detección y corrección de errores, balance de tiempos y ajustes de dificultad.

- **Optimización de Rendimiento:** Ajustes para mejorar el rendimiento en distintas plataformas y pruebas de estabilidad.

### 4. Fase de Preparación y Lanzamiento: Gold Master (2-3 semanas)
- **Marketing y Redes Sociales:** Creación de contenido promocional y publicaciones previas al lanzamiento.

- **Lanzamiento y Soporte Inicial:** Preparación de la versión final y publicación en la plataforma seleccionada, así como respuesta inicial a retroalimentación de usuarios.

## Costes asociados
- **Costes Fijos:** Los principales costos fijos incluyen los salarios del equipo de desarrollo, los gastos de hosting y servidores, y las licencias de software necesarias para la creación del juego. Estos costos son esenciales para el funcionamiento continuo del proyecto.

- **Costes Variables:** Los gastos en marketing y publicidad pueden variar según las campañas activas y las colaboraciones con influencers. También habrá costos asociados a la creación de eventos especiales dentro del juego, así como la producción de contenido cosmético.

# 4. MECÁNICAS DE JUEGO Y ELEMENTOS DE JUEGO
## Descripción detallada del concepto de juego
El jugador controla a Ashen, quien se desplaza por el escenario en una escoba encantada, lo que le permite moverse de manera ágil y cómoda para atender los pedidos de los vecinos. Al acercarse a cada habitante, Ashen puede evaluar las misiones y decidir cuándo ayudar. Los recados requieren dirigirse a puntos específicos del mapa para recoger objetos o completar minijuegos, y algunos pueden implicar volver a la ubicación del personaje que pidió ayuda. Para optimizar sus desplazamientos, el jugador puede recoger ciertos objetos que recargan una barra de impulso visible en el HUD, permitiéndole ganar velocidad de manera temporal. Este impulso es activable a voluntad, pero se consume cuando está en uso, por lo que debe usarse estratégicamente.

La partida inicia con un tiempo límite (por ejemplo, 60 segundos), y cada misión completada agrega segundos extra, permitiendo que el jugador continúe en acción. El desafío consiste en completar tantas misiones como sea posible antes de que el tiempo llegue a cero, maximizando así su tiempo en partida.

## Descripción Detallada de las Mecánicas de Juego
### 1. Movimiento y Desplazamiento
- **Control de la Escoba:** El jugador controla a Ashen mientras se desplaza por el escenario en su escoba. La escoba permite un movimiento fluido y ágil en diferentes direcciones (adelante, atrás, izquierda, derecha) con opción de activar un impulso para mayor velocidad.

- **Movimiento a Pie:** Además de desplazarse en escoba, Ashen puede caminar. Esta mecánica proporciona flexibilidad al jugador para interactuar con los NPCs y explorar el entorno a un ritmo más pausado.

- **Impulso Temporizado:** A lo largo del escenario, el jugador puede recolectar objetos o completar misiones que rellenan una barra de impulso. Este impulso ofrece una ventaja de velocidad limitada que se agota al usarse, regresando al desplazamiento normal hasta que la barra se recargue nuevamente.

### 2. Sistema de Misiones
- **Interacción con NPCs:** Para activar las misiones, el jugador debe acercarse a ciertos NPCs repartidos por la ciudad. Estos personajes le solicitarán ayuda con diversos recados, como recoger un objeto o realizar una tarea específica.

- **Tipos de Misiones:**
    - **Recoger Objetos:** Desplazarse a un punto específico para obtener un objeto y regresarlo al NPC que lo solicitó.
  
    - **Minijuegos de Habilidad:** Algunas tareas pueden incluir minijuegos rápidos que desafían la habilidad o precisión   del jugador, como preparar un objeto o realizar una acción específica en un tiempo determinado.

    - **Recompensas por Completar Misiones:** Al completar una misión, el jugador gana segundos extra en el temporizador principal, permitiéndole extender su tiempo de juego y tomar más recados.
 
### 3. Sistema de Tiempo y Reloj
- **Temporizador Principal:** La partida comienza con un tiempo limitado que representa el reloj principal. Este temporizador decrece continuamente durante el juego.

- **Aumento de Tiempo por Misión Completa:** Al completar cada misión, el jugador obtiene tiempo adicional en el reloj, lo cual es fundamental para prolongar la partida. El objetivo es maximizar el tiempo activo en el juego completando tantas misiones como sea posible.

- **Final del Juego:** Si el tiempo llega a cero, la partida finaliza automáticamente, y el jugador puede ver la tabla de clasificación.

### 4. Sistema de *Power-ups*
Distribuidos por el escenario podemos encontrar diferentes objetos de impulso, estos objetos recargan la barra de impulso cuando se recolectan, permitiendo que el jugador pueda activar el modo de desplazamiento rápido para cumplir misiones más eficientemente. Para usarlo, el jugador debe pulsar la tecla *shift*, si no la pulsa el personaje avanzará más lentamente pero no gastará el *boost*.

### 5. Tienda de Aspectos
- **Personalización del Personaje:** Los jugadores pueden visitar una tienda dentro del juego para adquirir aspectos estéticos para su personaje (cabeza, parte superior, parte inferior, zapatos). Estas mejoras son puramente estéticas y no afectan las mecánicas de juego.

- **Monedas del Juego:** Los aspectos se compran con monedas que se obtienen jugando, viendo anuncios o mediante micropagos, permitiendo que los jugadores personalicen la apariencia de Ashen a su gusto.

### 6. Controles y compatibilidad multiplataforma
- **Plataforma de Juego:** Al ser un juego de navegador, se puede jugar tanto en ordenador como en dispositivos móviles, ofreciendo una experiencia accesible para todos los usuarios.

- **Controles:**
    - **PC:** Navegación de Menús: El jugador puede utilizar el ratón para interactuar con los menús y seleccionar los botones de la interfaz.

    - **Gameplay:**
        - **Movimiento del Personaje:** Se utilizan las teclas WASD para mover al personaje.
        - **Correr:** La tecla Shift permite al jugador correr y desplazarse más rápidamente.
        - **Interacción:** La tecla E se usa para interactuar con objetos y personajes en el entorno.
        - **Volar:** La tecla Space permite al jugador volar con su escoba.
        - **Avance de Texto:** La tecla Tab se usa para avanzar el texto durante los diálogos y las interacciones.

    - **Dispositivos Móviles:** El juego se controla de forma táctil, utilizando un joystick en pantalla para el movimiento del personaje. Los jugadores pueden realizar taps en la pantalla para interactuar con objetos y personajes, así como para acceder a menús.

### 7. Tabla de Clasificación
La tabla de ranking es una característica esencial de Wicked Winds, diseñada para fomentar la competencia y el progreso entre los jugadores. Para acceder a esta funcionalidad, los jugadores deben crear una cuenta o iniciar sesión si ya poseen una. Esto permite a los usuarios llevar un seguimiento de su rendimiento y compararlo con el de otros jugadores.

- **Creación de Cuenta:** Al iniciar el juego por primera vez, los jugadores tienen la opción de registrarse, proporcionando un nombre de usuario y una dirección de correo electrónico. Este proceso es rápido y sencillo, permitiendo una integración fluida en la comunidad del juego.

- **Iniciar Sesión:** Los jugadores que ya tienen una cuenta pueden iniciar sesión de forma fácil, asegurando que sus progresos y logros se guarden y se reflejen en la tabla de clasificación.

- **Visualización de la Clasificación:** Una vez que los jugadores han iniciado sesión, pueden acceder a la tabla de ranking, donde se muestran las puntuaciones de todos los jugadores. Esta tabla se actualiza regularmente, permitiendo que los jugadores vean su posición actual y se motiven a mejorar su rendimiento en las misiones.

La inclusión de la tabla de ranking no solo añade un elemento de competencia, sino que también permite a los jugadores conectar con otros en la comunidad y compartir sus logros.

# 5. TRASFONDO
## Descripción Detallada de la Historia y la Trama
Ashen es un joven brujo audaz y servicial que se ha convertido en el héroe no oficial de su ciudad mágica. Con su escoba siempre a mano, ayuda a sus vecinos con problemas que van desde la entrega de ingredientes místicos hasta la recolección de objetos mágicos. La ciudad es un lugar vibrante y encantado, habitado por personajes excéntricos con historias sorprendentes que influyen en las tareas que le piden a Ashen.

La historia se desarrolla a través de interacciones con estos personajes y mediante diálogos enriquecidos que revelan fragmentos del trasfondo de la ciudad y de la vida de los habitantes. Cada recado o misión desvela una pequeña pieza de la narrativa del juego, y mientras el jugador ayuda a los ciudadanos, también explora los secretos ocultos del lugar y se encuentra con personajes enigmáticos que añaden un toque de misterio y posibles tramas ocultas.

## Personajes
- **Ashen - El protagonista:** Un joven brujo que ha ganado reputación por su habilidad para resolver problemas. Alegre y siempre dispuesto a ayudar, utiliza su escoba mágica para moverse por la ciudad. Su personalidad enérgica y curiosa le permite ganarse la confianza de los habitantes.

- **Habitantes de la ciudad:** Cada NPC tiene características y necesidades únicas; algunos son cómicos, otros misteriosos, cada uno con tareas relacionadas con sus personalidades. Las interacciones con ellos pueden revelar conexiones sorprendentes entre sus historias.

- **Personajes especiales:** A lo largo del juego, Ashen se cruzará con personajes enigmáticos que revelan secretos de la ciudad y ofrecen misiones especiales con grandes recompensas, enriqueciendo la experiencia del jugador.

## Entornos y Lugares
- **Ciudad mágica:** Inspirada en la arquitectura medieval, con influencias mágicas. Las calles están adornadas con farolas, árboles, calderos mágicos y edificaciones que dan un toque fantástico.

- **Plaza central:** Uno de los lugares más concurridos, donde los habitantes suelen congregarse. Aquí suelen comenzar y terminar varias misiones.

- **Mercadillo:** Ubicado en la plaza central, lleno de puestos de vendedores de ingredientes y herramientas mágicas, es uno de los lugares favoritos de Ashen para realizar sus recados.

- **Lugares adicionales:** El bosque encantado, el pantano y la torre de hechicería ofrecen más oportunidades de exploración y aventuras que conectan con la narrativa principal.

# 6. ARTE
## Estética General del Juego
La estética de Wicked Winds se basa en un estilo cartoon en 3D de *low-poly*, diseñado para capturar una ambientación colorida y mágica. Se emplean colores vibrantes y efectos luminosos suaves, creando una atmósfera envolvente que refleja la naturaleza mágica de la ciudad y su encanto sobrenatural. Esta elección estética no solo atrae visualmente, sino que también establece el tono del juego, invitando a los jugadores a sumergirse en un mundo de fantasía.

## Apartado Visual
El diseño de los personajes y del entorno sigue una línea visual simple, caracterizada por formas estilizadas y texturas planas. Los colores son saturados lo que contribuye a crear un estilo visual homogéneo y atractivo. Esta simplicidad en el diseño permite que la atención se centre en la jugabilidad y las interacciones, manteniendo al mismo tiempo un enfoque estético agradable.

# 7. INTERFAZ
La interfaz del juego es minimalista y se centra en la funcionalidad, evitando distracciones que puedan apartar al jugador de la acción principal. Durante la partida, el HUD (Head-Up Display) presenta el joystick en el lado izquierdo de la pantalla, permitiendo un control intuitivo del movimiento de Ashen. En el lado derecho, se encuentran los botones para interactuar y volar, facilitando la ejecución de acciones de manera rápida y eficiente.

En la parte superior de la pantalla, se muestra el tiempo restante y el medidor de impulso, lo que permite al jugador gestionar su tiempo y recursos con claridad. Cuando el jugador interactúa con los NPCs, aparece un cuadro de diálogo que muestra el nombre del personaje y las instrucciones correspondientes, asegurando una comunicación efectiva durante las misiones.

Además, el HUD incluye un botón que lleva al menú de pausa, lo que facilita el control y la navegación en el juego.

# 8. HOJA DE RUTA DEL DESARROLLO
<p align="center">
  <img src="https://i.postimg.cc/wjf4mngQ/TIME-LINE-5.png">
</p>

### HITO 1 (3 MESES): Personajes nuevos e implementación de habilidades.
- **Desarrollo de personajes:** Crear 5 personajes únicos que tengan un diseño coherente con el estilo artístico (3D lowpoly y cartoon). Cada uno debe tener una personalidad visual y una habilidad que ofrezca distintas ventajas durante el juego, como diferentes formas de impulso o poderes mágicos.
- **Implementación de habilidades:** Cada personaje debe tener una habilidad especial que aporte algo diferente a la mecánica de completar recados o desplazarse por la ciudad (ej. un personaje con mayor velocidad, otro con la habilidad de atraer más power-ups). ashen también tendrá una habilidad ya que antes de esta actualización no tendría. A partir de este punto se podrán incluir en la tienda diferentes skins para estos nuevos personajes.

### HITO 2 (6 MESES): Mapas nuevos
- **Diseño de mapas:** Crear 3 nuevas áreas, cada una con una temática única. Esto puede incluir zonas como un poblado helado, un tétrico bosque o una zona desértica.
- **Mecánicas únicas de los mapas:** estos nuevos mapas tendrán mecánicas especiales tales como un fuerte viento que aparece aleatoriamente en algunas calles y puede dificultar el desplazamiento.

### HITO 3 (9 MESES): Power Ups, Pase de batalla y Misiones.
- **Power-ups:** Introducir una variedad de nuevos power-ups que ofrezcan mecánicas adicionales como un boost de velocidad, escudos protectores, y habilidades de recolección automática de recados.
- **Pase de Batalla:** Desarrollar un sistema de recompensas por temporadas que motive a los jugadores a cumplir con objetivos específicos para desbloquear cosméticos, personajes nuevos o monedas del juego. Este pase tendrá una parte gratuita y patria de pago, siendo la de pago la que mejores cosas ofrezca.
- **Misiones:** Al incluirse el pase, se añadirán misiones tanto diarias como semanales con las que el jugador pueda conseguir puntos para seguir avanzando en el pase.

### HITO 4 (1 AÑO): Modo multijugador colaborativo y Housing
- **Multijugador colaborativo:** Implementar un sistema donde los jugadores puedan trabajar juntos para completar recados en equipo. Este modo podría incluir la posibilidad de repartir tareas entre jugadores o compartir power-ups. Sería un modo con un ranking diferente al del modo singleplayer y se jugaría cooperativo con un amigo o mediante emparejamiento aleatorio.
- **Modo Housing:** Crear una función donde los jugadores puedan personalizar su propia casa dentro de la ciudad, desbloqueando decoración y objetos según progresen en el juego. Además los jugadores que tengas agregados a tu lista de amigos podrán visitar tu casa. Los muebles y decoraciones se podrán comprar con la moneda del juego y algunos se darán de forma exclusiva en el pase de batalla.

### HITO 5 (15 MESES): Eventos temporales (Halloween, Navidad, etc.)
- **Eventos especiales:** Crear eventos temáticos donde la ciudad cambie de ambiente y se ofrezcan misiones y recompensas exclusivas de tiempo limitado (ej. misiones de temática de Halloween que den distintas recompensas como muebles, skins o monedas del juego).
- **Contenido estacional:** Añadir elementos visuales como pequeñas decoraciones navideñas en el mapa durante el evento de Navidad. También en la tienda se podrán comprar distintos muebles y skins de la temática del evento.

### HITO 6 (18 MESES): Modos multijugador competitivos
- **Modos competitivos:** Crear nuevos modos donde los jugadores compitan directamente, como un modo donde los jugadores se enfrenten para ver quien consigue hacer más recados en un tiempo límite (jugando cada uno en su mapa), un modo donde cada cierto tiempo se vayan eliminando a los jugadores con menor puntuación hasta que solo quede uno o un modo donde 4 o 5 jugadores jueguen en el mismo mapa y puedan estorbarse el uno al otro mediante el uso de habilidades u objetos.
- **Ranking de ligas:** El principal de estos modos tendría un ranking especial, ya que sería un ranking dividido en diferentes ligas (bronce, plata, oro, etc) donde para ascender de liga tendrás que alcanzar un número de puntos y ganar en las partidas de ascenso.

### HITO 7 (21 MESES): Merchandising del juego
- **Diseño de productos:** Desarrollar artículos promocionales como camisetas, figuras de personajes, pósters y otros productos basados en el juego.
- **Tienda en línea:** Crear una plataforma dentro de la página oficial del juego donde los fans puedan comprar el merchandising oficial.
- **Marketing:** Promocionar el merchandising a través de las redes sociales oficiales del juego y dentro del propio juego.

### HITO 8 (2 AÑOS): Creación de un torneo competitivo
- **Torneo oficial:** Organizar un torneo presencial donde los jugadores compitan por premios y reconocimiento, centrado en los modos competitivos introducidos en el Hito 6.
- **Reglas:** Definir reglas claras para la competición que hagan que esta sea lo más justa posible así como evitar diversas trampas por parte de los competidores.
- **Promoción y difusión:** Publicitar el evento en redes sociales y con la comunidad del juego para atraer la mayor cantidad de jugadores posible.

# 9. MODELO DE NEGOCIO
## Información de usuarios
#### Perfil demográfico
Wicked Wings es un juego casual multiplataforma, cuyo público posee un rango de edad bastante amplio (10 - 40 años) y un género mixto, debido a que es una experiencia dinámica y amena que puede ser disfrutada por cualquier persona. Sin embargo, el rango de más alto interés puede estar entre los 14 - 25, debido a las mecánicas de juego de gestión del tiempo. Se espera principalmente el éxito del videojuego en América, Europa y Asia, donde los juegos de navegador y móviles tienen una alta tasa de adopción.
#### Perfil psicográfico
Los jugadores muestran fascinación por temas de magia y fantasía además de por el género arcade. Prefieren juegos que puedan disfrutar en sesiones cortas (de 5 a 15 min), lo que encajan con las misiones rápidas y el ritmo acelerado del juego. Tienden a ser personas competitivas tanto con el resto de jugadores como con ellos mismos debido al sistema de puntuaciones y récords globales.
#### Hábitos de consumo
En cuanto a sus hábitos de consumo se esperan jugadores que frecuenten los juegos de navegador y que hagan microtransacciones para obtener aspectos para el personaje, escobas personalizadas o efectos especiales.

## Mapa de empatía
Es importante definir el mapa de empatía que nos permita empatizar con el jugador y diseñar una experiencia que lo enganche emocionalmente, anticipando sus motivaciones y posibles frustraciones.

### ¿Con quién estamos empatizando?
#### ¿Quién es la persona que queremos entender?
- Jugadores adolescentes y jóvenes adultos, con interés en juegos rápidos, de acción y temática mágica.
#### ¿Cuál es la situación en la que se encuentran?
- Buscan entretenimiento casual y competitivo, en cortos periodos de tiempo, en plataformas de fácil acceso como móviles y navegadores.
#### ¿Cuál es su papel en la situación?
- El jugador asume el rol de ashen, una bruja aprendiz que completa misiones bajo presión de tiempo para escalar en las clasificaciones y desbloquear nuevos desafíos.

### ¿Qué necesitan hacer?
#### ¿Qué necesitan hacer de manera diferente?
- Optimizar el uso del tiempo en el juego, planificar rutas eficientes y ejecutar bien los minijuegos para mantener el cronómetro corriendo y escalar en las clasificaciones.
#### ¿Qué trabajo necesitan o desean realizar?
- Completar misiones rápidas y variadas, como entregar cartas, preparar pociones o rescatar gatos, para mantenerse en acción y progresar en el juego.
#### ¿Qué decisiones necesitan tomar?
- Priorizar qué misiones realizar primero, maximizar el tiempo extra ganado en cada tarea, y decidir cuándo invertir en personalizaciones o mejoras.
#### ¿Cómo sabremos que han sido exitosos?
- Cuando logren superar sus mejores tiempos, escalar en la clasificación.

### ¿Qué ven?
#### ¿Qué ven en el mercado?
- Juego arcade y de acción con mecánicas rápidas y competitivas, como Crazy Taxi o Temple Run, y otros juegos con componentes mágicos y de exploración.
#### ¿Qué ven en su entorno inmediato?
- Un juego visualmente atractivo con un estilo vibrante y caricaturesco, con un mundo lleno de detalles mágicos, misiones dinámicas y una interfaz intuitiva.

### ¿Qué ven que otros están diciendo y haciendo?
- Influencers y jugadores hablando de estrategias para completar misiones más rápido, mostrando sus mejores puntajes en redes sociales y compartiendo logros estéticos como skins de escobas y atuendos.
#### ¿Qué están viendo y leyendo?
- Tutoriales o videos en plataformas como YouTube y TikTok sobre cómo maximizar el tiempo en las misiones, mejorar en los minijuegos y gestionar mejor las rutas.

### ¿Qué oyen?
#### ¿Qué están escuchando de los demás?
- Opiniones de amigos y compañeros de juegos sobre la competencia en las clasificaciones y el atractivo de la personalización de personajes.
#### ¿Qué están oyendo de amigos?
- Recomendaciones sobre cómo mejorar sus tiempos o estrategias en minijuegos.
- Retroalimentación constante a través de sonidos mágicos y efectos sonoros cuando completa misiones o gana tiempo extra.
- La música de fondo envolvente, que refuerza la temática de magia y urgencia, ayudando a mantener el ritmo rápido del juego.

### ¿Qué piensan y sienten?
#### ¿Cuáles son sus miedos, frustraciones y ansiedades?
- Temen que el juego se vuelva repetitivo si no hay suficiente variedad en las misiones o mapas.
- Se frustran cuando pierden porque no logran completar una misión a tiempo, o si la curva de dificultad es demasiado abrupta.
#### ¿Cuáles son sus deseos, necesidades, esperanzas y sueños?
- Esperan desbloquear personalizaciones únicas para ashen y sus habilidades mágicas, como skins de escobas o nuevas misiones y áreas en el mapa.
- Sueñan con escalar a lo más alto de las clasificaciones, mostrando sus habilidades para gestionar el tiempo y dominar los minijuegos.
- Quieren un flujo constante de nuevas actualizaciones, eventos o retos que mantengan el juego fresco e interesante.

## Caja de herramientas
### Bloques
- **La empresa (Loomlight):** Desarrolladora indie responsable de la creación, publicación y monetización del juego.
- **Proveedores (Recursos tecnológicos y financieros):** Empresas que proporcionan diferentes tecnologías; motores de juego (Unity), servicios en la nube, herramientas de monetización, y posibles financiadores.
- **Consumidores (Jugadores):** Público objetivo que interactúa con el juego, desde jugadores casuales hasta fans de juegos arcade y mágicos en plataformas de navegador.
- **Gobierno/Reguladores:** Autoridades que regulan aspectos como protección de datos de los jugadores, clasificación por edades y políticas de monetización.
- **Otras empresas (Plataformas de distribución y publicidad):** Webs o servicios que ofrecen visibilidad para el juego; plataformas de distribución de juegos para PC (Itch.io) y páginas web donde jugador en navegador.

### Relaciones
- **Productos (Empresa - Consumidores):** El principal producto es el propio juego. Se comercializa como un juego de navegador con mecánicas arcade/acción y misiones rápidas.
- **Servicios (Empresa - Consumidores/Proveedores):** La empresa puede ofrecer servicios como soporte técnico, actualizaciones constantes del juego y contenido adicional a través de microtransacciones o DLCs.
- **Experiencia (Consumidores - Empresa):** La experiencia del usuario es crucial. Wicked Winds busca ofrecer una experiencia mágica e inmersiva, con controles intuitivos, variabilidad de misiones y entornos atractivos. El feedback de los jugadores será clave para futuras actualizaciones.
- **Visibilidad (Empresa - Otras empresas/Consumidores):** Estableciendo estrategias de marketing daremos visibilidad al juego; anuncios en redes sociales, colaboraciones con influencers o participación en eventos de videojuegos.
- **Exposición (Empresa - Gobierno/Otras empresas):** La exposición del juego dependerá de cumplir con las normativas de distribución; edad recomendada o protección de datos. Para ganar exposición inicial será importante darnos a conocer en plataformas de distribución.
- **Dinero/Poco Dinero (Empresa - Consumidores/Otras empresas):** Se plantea un modelo de monetización freemium con micropagos para la obtención de monedas que se podrán canjear por aspectos y personalización. También es posible conseguirlas a través de publicidad y anuncios no intrusivos, aunque en menor cantidad.
- **Información (Empresa - Consumidores/Proveedores):** La empresa recogerá datos sobre el comportamiento de los jugadores para mejorar la jugabilidad y adaptarlo a las posibles necesidades que puedan surgir, para así captar a un mayor número de jugadores. Los proveedores tendrán el papel de ofrecer estadísticas de rendimiento.
- **Derechos (Empresa - Otras empresas/Proveedores):** Loomlight mantiene los derechos de propiedad intelectual, aunque se tienen licencias con proveedores por el uso de herramientas de software y canales de distribución; Unity o Itch.io, respectivamente.
- **Crédito (Empresa - Proveedores):** La empresa obtendrá financiamiento inicial a través de préstamos o créditos de los proveedores. El modelo freemium que hemos elegido, generará ingresos mediante micropagos y publicidad.

### Ejemplo del proceso
1. La empresa Loomlight crea y publica Wicked Winds como un juego de navegador y dispositivos móviles.
2. Los consumidores (jugadores) pueden acceder al juego gratuitamente y, opcionalmente, podrán realizar micropagos o visualizar anuncios para obtener recompensas, meramente estéticas, dentro del juego. A través de estos, la empresa genera ingresos.
3. Los proveedores (Unity) proporcionan las tecnologías necesarias para desarrollar el juego.
4. Otras empresas (plataformas de distribución como Itch.io o Play Store) promueven el juego en sus plataformas y comparten parte de las ventas o ingresos publicitarios.
5. El gobierno y reguladores aseguran que la empresa cumple con las normativas de protección de datos y regulación del mercado de juegos.

## Modelo de lienzo

### Segmento de Clientes
Wicked Winds está diseñado para atraer a un grupo de jugadores que buscan una experiencia rápida y accesible en un entorno mágico, sin las complicaciones típicas de los juegos descargables. Nuestros segmentos clave incluyen:
- **Jugadores Casual de Navegador:** Este subgrupo está compuesto por personas que juegan de manera ocasional y que prefieren juegos accesibles desde el navegador, sin necesidad de descargar aplicaciones adicionales. Son jugadores que buscan entretenimiento rápido en sus momentos libres, ya sea en sus ordenadores o dispositivos móviles, sin compromiso con sesiones largas.
- **Jugadores Competitivos de Bajo Estrés:** Jugadores que disfrutan la competitividad, pero sin las tensiones asociadas con los juegos multijugador en tiempo real. Prefieren el desafío personal y la progresión basada en rankings globales. Les atraen los juegos que ofrecen recompensas cosméticas sin influir en el rendimiento, lo que les permite competir sin pagar por ventajas injustas.

### Propuestas de Valor
- **Experiencia de Fantasía Inmediata y Accesible:** Wicked Winds transporta a los jugadores directamente a un mundo mágico desde su navegador. No requiere instalaciones ni configuraciones complejas. Ofrecemos una experiencia visual vibrante y envolvente que combina la magia, la fantasía y la rapidez, permitiendo a los jugadores convertirse en ashen, una aprendiz de bruja, realizando misiones por la aldea.

- **Competencia Sin Estrés y Personalización:** El juego ofrece una clasificación competitiva que permite a los jugadores mejorar a su propio ritmo. No es necesario enfrentarse a otros en tiempo real, eliminando así la presión excesiva. Las recompensas son cosméticas, lo que significa que los jugadores pueden personalizar a ashen y su escoba sin que ello afecte el desempeño en el juego.

- **Jugar Gratis, Con Opciones de Compra:** El contenido principal del juego es completamente gratuito, incluidas las misiones y eventos especiales. Para aquellos jugadores que quieran destacar con un estilo único, ofrecemos opciones de personalización y aspectos cosméticos a precios accesibles. Las compras son opcionales y no ofrecen ventajas competitivas, garantizando que todos los jugadores compitan en igualdad de condiciones.

- **Personalización Visual Única:** Los jugadores pueden expresar su estilo personal a través de la apariencia de ashen y su escoba. Esto les permite destacarse y hacer que su experiencia de juego sea única, sin influir en las mecánicas de juego o rendimiento, lo que asegura una competencia justa.

### Canales
- **Redes Sociales:** Las plataformas como Youtube y X (Twitter) serán clave para interactuar con los jugadores, compartiendo contenido visual del juego, actualizaciones, y memes relacionados con la temática mágica de Wicked Winds. Se planea utilizar estas plataformas para lanzar campañas de anuncios y eventos promocionales.

- **Eventos Temáticos en el Juego:** Wicked Winds planea organizar eventos especiales, como misiones temáticas en fechas como Halloween o Navidad. Estos eventos serán gratuitos, ofreciendo a los jugadores la oportunidad de desbloquear aspectos limitados, aumentando su conexión con el juego y fomentando la participación activa.

- **Streaming en Vivo:** Se buscarán colaboraciones con streamers populares en plataformas como Twitch y YouTube para que jueguen Wicked Winds, mostrando su competitividad y compartiendo sus impresiones en vivo. Este enfoque ayudará a aumentar la visibilidad del juego y atraerá a una nueva audiencia.

- **Comunidades en Línea:** Crear foros oficiales, grupos de Discord y subreddits permitirá que los jugadores compartan sus experiencias, estrategias, fanart, y sugerencias para el juego. Fomentar la interacción entre los jugadores y el equipo de desarrollo asegurará una retroalimentación continua y una comunidad activa.

- **Boletines Electrónicos:** Un boletín mensual mantendrá informada a la comunidad sobre las novedades del juego, incluyendo teasers de nuevos aspectos, historias cortas del mundo de Stardust Town, y anuncios de eventos especiales. Esto mantendrá a los jugadores conectados con el juego incluso cuando no estén jugando.

### Relaciones con los Clientes
- **Asistencia Directa y Personalizada:** Ofreceremos soporte a los jugadores a través de chats en línea y correos electrónicos, asegurándonos de que cualquier problema técnico o consulta sea resuelto rápidamente. Queremos que cada jugador sienta que tiene un apoyo confiable para mejorar su experiencia de juego.
 
- **Comunidad Activa y Colaborativa:** Fomentamos la creación de una comunidad donde los jugadores puedan compartir contenido, ideas y sugerencias. A través de encuestas, los jugadores también podrán participar en decisiones sobre futuras actualizaciones, haciéndolos sentir parte del desarrollo del juego.
  
- **Notificaciones y Promociones Personalizadas:** Se utilizarán notificaciones automáticas dentro del juego para alertar a los jugadores sobre eventos, promociones y actualizaciones. Esta estrategia se adaptará a las preferencias de cada jugador, maximizando el engagement sin ser invasiva.

### Fuentes de Ingreso
- **Venta de Activos Cosméticos:** El principal ingreso proviene de la venta de aspectos cosméticos. Los jugadores pueden comprar skins para ashen, nuevas escobas con efectos visuales únicos, o trajes temáticos. Todo el contenido cosmético no afecta el rendimiento del juego, manteniendo la competencia justa.

- **Colaboraciones y Publicidad:** La publicidad en la tienda para conseguir más monedas genera beneficios económicos. Además, habrá oportunidades para introducir publicidad no intrusiva en eventos especiales patrocinados o colaboraciones con marcas que se alineen con la temática del juego. Estas colaboraciones pueden ofrecer objetos de marca dentro del juego.

### Recursos Clave
- **Recursos Físicos:** Necesitamos servidores robustos y servicios en la nube que puedan manejar el tráfico en línea y garantizar un rendimiento rápido y estable. Además, contar con oficinas o espacios colaborativos para el equipo de desarrollo es esencial.

- **Recursos Intelectuales:** El equipo debe proteger la propiedad intelectual del juego, incluidos los diseños de personajes, las mecánicas de juego, y la narrativa del mundo mágico. También es vital desarrollar una marca sólida que resuene con la audiencia y genere lealtad.

- **Recursos Humanos:** Nuestro equipo incluye desarrolladores, artistas, diseñadores de juegos y escritores, así como profesionales de marketing y soporte al cliente. Este equipo multidisciplinario es clave para desarrollar, mantener y promocionar el juego.

- **Recursos Económicos:** El financiamiento inicial y el acceso a capital adicional serán necesarios para cubrir los costos de desarrollo, infraestructura tecnológica, y campañas de marketing. Esto asegura que podamos seguir desarrollando y actualizando el juego después de su lanzamiento.

### Actividades Clave
- **Desarrollo Continuo del Juego:** Además de crear el juego base, es esencial realizar actualizaciones frecuentes, agregar contenido nuevo y corregir errores que puedan surgir. Mantener el interés de los jugadores requiere una evolución constante del mundo y las misiones de Wicked Winds.
  
- **Marketing y Promoción:** Implementar campañas de marketing digital efectivas de autopromoción, y en un futuro colaborando con influencers y creando eventos en redes sociales que impulsen la visibilidad del juego y atraigan a nuevos jugadores.
  
- **Gestión de la Comunidad:** Desarrollar y mantener una comunidad activa es clave para la longevidad del juego. A través de eventos, foros y encuestas, nos aseguramos de que los jugadores se sientan involucrados y escuchados.
  
- **Soporte y Feedback:** Ofrecer soporte técnico y atención al cliente es fundamental para asegurar que los jugadores disfruten de una experiencia sin interrupciones. También se trabajará en la recopilación de feedback de los jugadores para implementar mejoras continuas.

### Asociaciones Clave
- **Colaboración con Plataformas de Juegos en Línea:** Asociarnos con plataformas de juegos como Itch.io o Kongregate nos permitirá aprovechar sus audiencias establecidas y atraer más jugadores a Wicked Winds. También podemos colaborar con marketplaces para ofrecer promociones conjuntas.

- **Socios en Marketing:** Trabajaremos con agencias de marketing digital especializadas en videojuegos para crear campañas dirigidas a los segmentos de jugadores casuales y competitivos. También colaboraremos con streamers e influencers de la industria para promocionar el juego.
  
- **Servicios Técnicos y Hosting:** Necesitamos proveedores confiables para gestionar los servidores y mantener el juego en línea sin interrupciones. Esto también incluye servicios de soporte técnico para resolver cualquier problema que surja durante el desarrollo y el mantenimiento del juego.

### Marketing
En principio, el marketing del juego se lleva a cabo por autopromoción a través de las redes sociales y página web de Loomlight, aunque si se consigue una buena repercusión, se espera que Wicked Winds aparezca en blogs especializados en juegos indie españoles. También se organizarían apariciones en pequeños eventos de juegos y colaboraciones con *influencers* de videojuegos o *streamers* que ayuden a aumentar la repercusión del juego.

## Métricas de éxito
Con estos tres escenarios se concretan las expectativas de las cifras a alcanzar con Wicked Winds:

### Escenario Pesimista
- **Base de Jugadores:** 5.000 usuarios activos mensuales.
- **Micropagos (Monedas):** Tasa de conversión del 0.3%, lo que equivale a 15 jugadores comprando monedas, con un gasto promedio de 1.50 € por compra.
- **Ingresos por Anuncios:** Ingresos muy bajos debido a pocos clics y visualizaciones en los anuncios, generando entre 20 € y 50 € al mes.
- **Ingresos Totales:** Entre 40 € y 70 € al mes (micropagos + anuncios).
- **Engagement:** Los jugadores encuentran poco atractiva la relación esfuerzo-recompensa, y los anuncios resultan molestos, aumentando la tasa de abandono.
- **Retención:** Solo un 5% de los jugadores siguen jugando después de 7 días.
- **Resultado:** El juego apenas genera ingresos suficientes para mantenerse y necesita ajustes importantes en su monetización o diseño.

### Escenario Normal
- **Base de Jugadores:** 25.000 usuarios activos mensuales.
- **Micropagos (Monedas):** Tasa de conversión del 1.5%, con 375 jugadores comprando monedas, con un gasto promedio de 2 € por compra.
- **Ingresos por Anuncios:** Ingresos moderados con una mayor cantidad de visualizaciones y clics en los anuncios, generando entre 200 € y 500 € al mes.
- **Ingresos Totales:** Entre 950 € y 1.250 € al mes (micropagos + anuncios).
- **Engagement:** Los jugadores interactúan con la tienda y las monedas adicionales obtenidas por micropagos, y toleran los anuncios, aunque algunos siguen abandonando el juego.
- **Retención:** Aproximadamente un 20% de los jugadores siguen jugando después de 7 días.
- **Resultado:** El juego es sostenible, cubre los costos operativos y permite actualizaciones periódicas de contenido y nuevas skins o ítems.

### Escenario Optimista
- **Base de Jugadores:** 100.000 usuarios activos mensuales.
- **Micropagos (Monedas):** Tasa de conversión del 3%, con 3,000 jugadores comprando monedas, con un gasto promedio de 3 € por compra.
- **Ingresos por Anuncios:** Ingresos elevados debido a la gran cantidad de impresiones y clics en los anuncios, generando entre 1.000 € y 2.000 € al mes.
- **Ingresos Totales:** Entre 10.500 € y 12.000 € al mes (micropagos + anuncios).
- **Engagement:** Los jugadores disfrutan de la posibilidad de personalizar su experiencia mediante las monedas adquiridas, y no se ven demasiado afectados por los anuncios, lo que mejora el compromiso con el juego.
- **Retención:** Un 35% de los jugadores permanecen después de 7 días.
- **Resultado:** El juego se convierte en un éxito moderado, generando ingresos suficientes para ampliar el contenido, agregar nuevas skins e ítems, y mejorar la experiencia de usuario.

# 10. Post-Mortem
## Aspectos Positivos

## Aspectos Negativos

## Lecciones aprendidas

## Conclusión

# This is an H1 header
## This is an H2 header
### This is an H3 header

*Italic text*
_Italic text_

**Bold text**
__Bold text__

***Bold and Italic text***

- Item 1
- Item 2
    - Subitem 2.1
    - Subitem 2.2
 
1. First item
2. Second item
    1. Subitem 2.1
    2. Subitem 2.2

[GitHub](https://github.com)

![Alt text](image-url.png)

> This is a quote.

Inline code: `example()`

```python
def example():
    print("This is a code block")
```

| Column 1 | Column 2 | Column 3 |
|----------|----------|----------|
| Row 1    | Data 1   | Data 2   |
| Row 2    | Data 3   | Data 4   |


- [x] Task 1 (Completed)
- [ ] Task 2 (Not completed)
