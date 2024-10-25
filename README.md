> ### Presentación
> Somos Loomlight, un grupo de estudiantes de diseño y desarrollo de videojuegos y creadores de Wicked Winds. Se trata de un proyecto conjunto entre varias asignaturas del curso con el que tratamos de crear un videojuego de navegador de internet que consiga ser completo y divertido, y nos permita acercarnos a la realidad del mercado de los videojuegos.

# Wicked Winds - Documento de Diseño de Videojuego

<div align="center"><b><i>
    Versión 0.0.3
</i></b></div>

<div align="center"><b>
    INTRODUCCIÓN
</b></div>

- **Título**: Wicked Winds
- **Desarrollador**: Loomlight
- **Género**: Juego de esitlo arcade/acción contrarreloj competitivo
- **Estilo artístico**: 3D *lowpoly* y *cartoon* con temática de magos
- **Pilares del diseño**: Inmersión, desafío y emoción
- **Plataforma**: Navegador de internet
- **Categoría**: Juego rápido de misiones, basado en la acumulación de tiempo, con una mecánica similar a [Crazy Taxi](https://es.wikipedia.org/wiki/Crazy_Taxi), pero ambientado en un mundo de magia.
- **Cámara**: 3D tercera persona
- **Periféricos**:
    - PC: Ratón
    - Dispositivo móvil: Pantalla táctil
- **Controles**: Al ser un juego de navegador y multiplataforma, los controles deben basarse en la interacción con su interfaz mediante *clicks*/*taps*. El movimiento del personaje jugable se controla mediante un *joystick* en pantalla.
- **Puntuación**: el jugador se clasifica en un *ranking* en línea junto a otros jugadores según el mayor tiempo que dure jugando.
- **Entorno**: La partida se produce en un escenario urbano que el jugador puede recorrer libremente.
- **Musica/Sonido**: La música y el diseño sonoro estarán enfocados en generar una atmósfera envolvente, con melodías dinámicas que se adapten al ritmo de la acción y sonidos que refuercen la tensión en momentos clave.

### Sinopsis
Ahen es un brujo que vive en una ciudad tan encantadora como bulliciosa. Los habitantes de esta siempre necesitan su ayuda para completar tareas urgentes, por lo que Ashen debe desplazarse lo antes posible de un punto a otro con su escoba para completar los recados antes de que el tiempo se acabe. Con cada recado completado, gana unos valiosos segundos para mantener el reloj en marcha, pero si el tiempo llega a cero, ¡el juego termina!

### Mecánica del juego
El jugador se desplaza por el escenario en una escoba encantada para atender cómodamente los recados de los vecinos. Debe acercarse a cada vecino para saber sus necesidades y decidir si es un buen momento para ayudarle. Cuando se activa un recado debe ir a otro punto del escenario para recoger algo o realizar una tarea en forma de minijuego, y luego es posible que deba volver a la ubicación de quien le pidió el recado. Para desplazarse más rápido el jugador deberá recoger ciertos objetos repartidos por el escenario que rellenarán poco a poco una barra en el HUD que indica el impulso que puede recibir. Este es limitado por lo que si se acaba se mueve a una velocidad normal. El bonus del impulso puede activarse o desactivarse como quiera el jugador y se gasta mientras está en uso.  

El jugador comienza con una cantidad fija de tiempo (ej. 60 segundos). Cada misión completada otorga segundos extra, permitiendo que el jugador siga en acción. Si el tiempo se agota, el juego termina. El objetivo es aguantar el máximo tiempo en partida completando tantas misiones como sea posible antes de que el reloj llegue a cero.

<div align="center"><b>
    MODELO DE NEGOCIO
</b></div>

## Monetización
Wicked Winds es un videojuego freemium, ya que es gratis pero incluye una tienda donde el jugador puede realizar micropagos para obtener más monedas que le permitan comprar ítems y *skins* para personalizar su partida. Cabe destacar que las compras realizadas en la tienda nunca supondrán una ventaja del jugador frente a los demás, ya que las clasificaciones de los jugadores deben ser justas.

Aparte de con micropagos, las monedas de tienda también se pueden conseguir: jugando, ya que se pueden encontrar unas pocas repartidas por el escenario en cada partida; o viendo anuncios, que aunque darán más monedas que jugando, sigue siendo una menor cantidad comparada con las que se pueden conseguir comprándolas directamente con micropagos.

En un futuro se podrían aumentar los beneficios económicos del juego con:
- Implementación de más tipos de monedas/objetos especiales, lo que permitiría justificar más micropagos opcionales.
- Creación y distribución de *merchandising* de Wicked Winds.
- Colaboraciones publicitarias con marcas conocidas que se alineen con la temática del juego.

<div align="center"><b>
    PLANIFICACIÓN Y COSTES
</b></div>

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

###Segmento de Clientes
Wicked Winds está diseñado para atraer a un grupo de jugadores que buscan una experiencia rápida y accesible en un entorno mágico, sin las complicaciones típicas de los juegos descargables. Nuestros segmentos clave incluyen:
1. Jugadores Casual de Navegador: Este subgrupo está compuesto por personas que juegan de manera ocasional y que prefieren juegos accesibles desde el navegador, sin necesidad de descargar aplicaciones adicionales. Son jugadores que buscan entretenimiento rápido en sus momentos libres, ya sea en sus ordenadores o dispositivos móviles, sin compromiso con sesiones largas.
2. Jugadores Competitivos de Bajo Estrés: Jugadores que disfrutan la competitividad, pero sin las tensiones asociadas con los juegos multijugador en tiempo real. Prefieren el desafío personal y la progresión basada en rankings globales. Les atraen los juegos que ofrecen recompensas cosméticas sin influir en el rendimiento, lo que les permite competir sin pagar por ventajas injustas.

### Propuestas de Valor
1. Experiencia de Fantasía Inmediata y Accesible: Wicked Winds transporta a los jugadores directamente a un mundo mágico desde su navegador. No requiere instalaciones ni configuraciones complejas. Ofrecemos una experiencia visual vibrante y envolvente que combina la magia, la fantasía y la rapidez, permitiendo a los jugadores convertirse en ashen, una aprendiz de bruja, realizando misiones por la aldea de Stardust Town.
2. Competencia Sin Estrés y Personalización: El juego ofrece una clasificación competitiva que permite a los jugadores mejorar a su propio ritmo. No es necesario enfrentarse a otros en tiempo real, eliminando así la presión excesiva. Las recompensas son cosméticas, lo que significa que los jugadores pueden personalizar a ashen y su escoba sin que ello afecte el desempeño en el juego.
3. Jugar Gratis, Con Opciones de Compra: El contenido principal del juego es completamente gratuito, incluidas las misiones y eventos especiales. Para aquellos jugadores que quieran destacar con un estilo único, ofrecemos opciones de personalización y aspectos cosméticos a precios accesibles. Las compras son opcionales y no ofrecen ventajas competitivas, garantizando que todos los jugadores compitan en igualdad de condiciones.
4. Personalización Visual Única: Los jugadores pueden expresar su estilo personal a través de la apariencia de ashen y su escoba. Esto les permite destacarse y hacer que su experiencia de juego sea única, sin influir en las mecánicas de juego o rendimiento, lo que asegura una competencia justa.

### Canales
1. Redes Sociales: Las plataformas como Youtube y X (Twitter) serán clave para interactuar con los jugadores, compartiendo contenido visual del juego, actualizaciones, y memes relacionados con la temática mágica de Wicked Winds. Se planea utilizar estas plataformas para lanzar campañas de anuncios y eventos promocionales.
2. Eventos Temáticos en el Juego: Wicked Winds planea organizar eventos especiales, como misiones temáticas en fechas como Halloween o Navidad. Estos eventos serán gratuitos, ofreciendo a los jugadores la oportunidad de desbloquear aspectos limitados, aumentando su conexión con el juego y fomentando la participación activa.
3. Streaming en Vivo: Se buscarán colaboraciones con streamers populares en plataformas como Twitch y YouTube para que jueguen Wicked Winds, mostrando su competitividad y compartiendo sus impresiones en vivo. Este enfoque ayudará a aumentar la visibilidad del juego y atraerá a una nueva audiencia.
4. Comunidades en Línea: Crear foros oficiales, grupos de Discord y subreddits permitirá que los jugadores compartan sus experiencias, estrategias, fanart, y sugerencias para el juego. Fomentar la interacción entre los jugadores y el equipo de desarrollo asegurará una retroalimentación continua y una comunidad activa.
5. Boletines Electrónicos: Un boletín mensual mantendrá informada a la comunidad sobre las novedades del juego, incluyendo teasers de nuevos aspectos, historias cortas del mundo de Stardust Town, y anuncios de eventos especiales. Esto mantendrá a los jugadores conectados con el juego incluso cuando no estén jugando.

### Relaciones con los Clientes
1. Asistencia Directa y Personalizada: Ofreceremos soporte a los jugadores a través de chats en línea y correos electrónicos, asegurándonos de que cualquier problema técnico o consulta sea resuelto rápidamente. Queremos que cada jugador sienta que tiene un apoyo confiable para mejorar su experiencia de juego.
2. Comunidad Activa y Colaborativa: Fomentamos la creación de una comunidad donde los jugadores puedan compartir contenido, ideas y sugerencias. A través de encuestas, los jugadores también podrán participar en decisiones sobre futuras actualizaciones, haciéndolos sentir parte del desarrollo del juego.
3. Notificaciones y Promociones Personalizadas: Se utilizarán notificaciones automáticas dentro del juego para alertar a los jugadores sobre eventos, promociones y actualizaciones. Esta estrategia se adaptará a las preferencias de cada jugador, maximizando el engagement sin ser invasiva.

### Fuentes de Ingreso
1. Venta de Activos Cosméticos: El principal ingreso proviene de la venta de aspectos cosméticos. Los jugadores pueden comprar skins para ashen, nuevas escobas con efectos visuales únicos, o trajes temáticos. Todo el contenido cosmético no afecta el rendimiento del juego, manteniendo la competencia justa.
2. Colaboraciones y Publicidad: La publicidad en la tienda para conseguir más monedas genera beneficios económicos. Además, habrá oportunidades para introducir publicidad no intrusiva en eventos especiales patrocinados o colaboraciones con marcas que se alineen con la temática del juego. Estas colaboraciones pueden ofrecer objetos de marca dentro del juego.

### Recursos Clave
1. Recursos Físicos: Necesitamos servidores robustos y servicios en la nube que puedan manejar el tráfico en línea y garantizar un rendimiento rápido y estable. Además, contar con oficinas o espacios colaborativos para el equipo de desarrollo es esencial.
2. Recursos Intelectuales: El equipo debe proteger la propiedad intelectual del juego, incluidos los diseños de personajes, las mecánicas de juego, y la narrativa del mundo mágico. También es vital desarrollar una marca sólida que resuene con la audiencia y genere lealtad.
3. Recursos Humanos: Nuestro equipo incluye desarrolladores, artistas, diseñadores de juegos y escritores, así como profesionales de marketing y soporte al cliente. Este equipo multidisciplinario es clave para desarrollar, mantener y promocionar el juego.
4. Recursos Económicos: El financiamiento inicial y el acceso a capital adicional serán necesarios para cubrir los costos de desarrollo, infraestructura tecnológica, y campañas de marketing. Esto asegura que podamos seguir desarrollando y actualizando el juego después de su lanzamiento.

### Actividades Clave
1. Desarrollo Continuo del Juego: Además de crear el juego base, es esencial realizar actualizaciones frecuentes, agregar contenido nuevo y corregir errores que puedan surgir. Mantener el interés de los jugadores requiere una evolución constante del mundo y las misiones de Wicked Winds.
2. Marketing y Promoción: Implementar campañas de marketing digital efectivas de autopromoción, y en un futuro colaborando con influencers y creando eventos en redes sociales que impulsen la visibilidad del juego y atraigan a nuevos jugadores.
3. Gestión de la Comunidad: Desarrollar y mantener una comunidad activa es clave para la longevidad del juego. A través de eventos, foros y encuestas, nos aseguramos de que los jugadores se sientan involucrados y escuchados.
4. Soporte y Feedback: Ofrecer soporte técnico y atención al cliente es fundamental para asegurar que los jugadores disfruten de una experiencia sin interrupciones. También se trabajará en la recopilación de feedback de los jugadores para implementar mejoras continuas.

### Asociaciones Clave
1. Colaboración con Plataformas de Juegos en Línea: Asociarnos con plataformas de juegos como Itch.io o Kongregate nos permitirá aprovechar sus audiencias establecidas y atraer más jugadores a Wicked Winds. También podemos colaborar con marketplaces para ofrecer promociones conjuntas.
2. Socios en Marketing: Trabajaremos con agencias de marketing digital especializadas en videojuegos para crear campañas dirigidas a los segmentos de jugadores casuales y competitivos. También colaboraremos con streamers e influencers de la industria para promocionar el juego.
3. Servicios Técnicos y Hosting: Necesitamos proveedores confiables para gestionar los servidores y mantener el juego en línea sin interrupciones. Esto también incluye servicios de soporte técnico para resolver cualquier problema que surja durante el desarrollo y el mantenimiento del juego.

### Estructura de Costos
1. Costes Fijos: Los principales costos fijos incluyen los salarios del equipo de desarrollo, los gastos de hosting y servidores, y las licencias de software necesarias para la creación del juego. Estos costos son esenciales para el funcionamiento continuo del proyecto.
2. Costes Variables: Los gastos en marketing y publicidad pueden variar según las campañas activas y las colaboraciones con influencers. También habrá costos asociados a la creación de eventos especiales dentro del juego, así como la producción de contenido cosmético.

#### Marketing
En principio, el marketing del juego se lleva a cabo por autopromoción a través de las redes sociales y página web de Loomlight, aunque si se consigue una buena repercusión, se espera que Wicked Winds aparezca en blogs especializados en juegos indie españoles. También se organizarían apariciones en pequeños eventos de juegos y colaboraciones con *influencers* de videojuegos o *streamers* que ayuden a aumentar la repercusión del juego.

### Métricas de éxito
Con estos tres escenarios se concretan las expectativas de las cifras a alcanzar con Wicked Winds:

#### Escenario Pesimista
- **Base de Jugadores:** 5,000 usuarios activos mensuales.
- **Micropagos (Monedas):** Tasa de conversión del 0.3%, lo que equivale a 15 jugadores comprando monedas, con un gasto promedio de 1,50 € por compra.
- **Ingresos por Anuncios:** Ingresos muy bajos debido a pocos clics y visualizaciones en los anuncios, generando entre 20 € y 50 € al mes.
- **Ingresos Totales:** Entre 40 € y 70 € al mes (micropagos + anuncios).
- **Engagement:** Los jugadores encuentran poco atractiva la relación esfuerzo-recompensa, y los anuncios resultan molestos, aumentando la tasa de abandono.
- **Retención:** Solo un 5% de los jugadores siguen jugando después de 7 días.
- **Resultado:** El juego apenas genera ingresos suficientes para mantenerse y necesita ajustes importantes en su monetización o diseño.

#### Escenario Normal
- **Base de Jugadores:** 25,000 usuarios activos mensuales.
- **Micropagos (Monedas):** Tasa de conversión del 1.5%, con 375 jugadores comprando monedas, con un gasto promedio de 2 € por compra.
- **Ingresos por Anuncios:** Ingresos moderados con una mayor cantidad de visualizaciones y clics en los anuncios, generando entre 200 € y 500 € al mes.
- **Ingresos Totales:** Entre 950 € y 1,250 € al mes (micropagos + anuncios).
- **Engagement:** Los jugadores interactúan con la tienda y las monedas adicionales obtenidas por micropagos, y toleran los anuncios, aunque algunos siguen abandonando el juego.
- **Retención:** Aproximadamente un 20% de los jugadores siguen jugando después de 7 días.
- **Resultado:** El juego es sostenible, cubre los costos operativos y permite actualizaciones periódicas de contenido y nuevas skins o ítems.

#### Escenario Optimista
- **Base de Jugadores:** 100,000 usuarios activos mensuales.
- **Micropagos (Monedas):** Tasa de conversión del 3%, con 3,000 jugadores comprando monedas, con un gasto promedio de 3 € por compra.
- **Ingresos por Anuncios:** Ingresos elevados debido a la gran cantidad de impresiones y clics en los anuncios, generando entre 1,000 € y 2,000 € al mes.
- **Ingresos Totales:** Entre 10,500 € y 12,000 € al mes (micropagos + anuncios).
- **Engagement:** Los jugadores disfrutan de la posibilidad de personalizar su experiencia mediante las monedas adquiridas, y no se ven demasiado afectados por los anuncios, lo que mejora el compromiso con el juego.
- **Retención:** Un 35% de los jugadores permanecen después de 7 días.
- **Resultado:** El juego se convierte en un éxito moderado, generando ingresos suficientes para ampliar el contenido, agregar nuevas skins e ítems, y mejorar la experiencia de usuario.

<div align="center"><b>
    MECÁNICAS Y ELEMENTOS DE JUEGO
</b></div>
<p align="center">
    
### Descripción detallada del concepto de juego
    
### Descripción detallada de las mecánicas de juego

### Controles

### Niveles y misiones

### Objetos, armas y power ups

### Arquitectura del software


<div align="center"><b>
    TRASFONDO
</b></div>
<p align="center">
    
### Descripción detallada de la historia y la trama
    
### Personajes

### Entornos y lugares


<div align="center"><b>
    ARTE
</b></div>
<p align="center">
    
### Estética general del juego
    
### Apartado visual

#### Concept Art
###Personaje

### Escenario

### Modelo 3D
#### Personaje

#### Escenario


<div align="center"><b>
    Interfaz
</b></div>
<p align="center">
    
### Diseños básicos de los menús
    
### Diagrama de flujo


<div align="center"><b>
    HOJA DE RUTA DEL DESARROLLO
</b></div>
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
