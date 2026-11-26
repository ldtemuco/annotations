create table if not exists "sectors" (
	"id"			integer not null,
	"course"		text,
	"sectorname"	text,	
	primary key("id" autoincrement)
);

insert into "sectors" values 
(null, '1a', 'formación general'),
(null, '1b', 'formación general'),
(null, '1c', 'formación general'),
(null, '1d', 'formación general'),
(null, '1e', 'formación general'),
(null, '1f', 'formación general'),
(null, '2a', 'formación general'),
(null, '2b', 'formación general'),
(null, '2c', 'formación general'),
(null, '2d', 'formación general'),
(null, '2e', 'formación general'),
(null, '3a', 'especialidad de: contabilidad'),
(null, '3b', 'especialidad de: administración'),
(null, '3c', 'especialidad de: programación'),
(null, '3d', 'especialidad de: gráfica'),
(null, '3e', 'especialidad de: gráfica'),
(null, '4a', 'especialidad de: contabilidad'),
(null, '4b', 'especialidad de: administración'),
(null, '4c', 'especialidad de: programación'),
(null, '4d', 'especialidad de: gráfica'),
(null, '4e', 'especialidad de: gráfica');


create table if not exists "heads" (
	"id"		integer not null,
	"course"	text,
	"boss"		text,	
	primary key("id" autoincrement)
);

insert into "heads" values 
(null, '1a', 'karla tripainao montecinos'),
(null, '1b', 'verónica romero mieres'),
(null, '1c', 'gabriela bravo escalona'),
(null, '1d', 'pedro acuña quijada'),
(null, '1e', 'guillermo marín cortéz'),
(null, '1f', 'emanuel araneda suazo'),
(null, '2a', 'maría josé acuña espinoza'),
(null, '2b', 'roberto sáez ramírez'),
(null, '2c', 'génesis epullán arévalo'),
(null, '2d', 'gabriela vega sáez'),
(null, '2e', 'karin olivares chávez'),
(null, '3a', 'alexandra scholz rodríguez'),
(null, '3b', 'alejandro ancalaf ancalaf'),
(null, '3c', 'luciano sepúlveda bravo'),
(null, '3d', 'clara morales cereceda'),
(null, '3e', 'claudia galaz opitz'),
(null, '4a', 'francisco orellana piña'),
(null, '4b', 'alejandro ancalaf ancalaf'),
(null, '4c', 'maría josé arteaga azócar'),
(null, '4d', 'consuelo montes inostroza'),
(null, '4e', 'francisco ferrada cornejo');

create table if not exists "1a" (
	"id"		integer not null,
	"subject"	text,
	"teacher"	text,
	primary key("id" autoincrement)
);
create table if not exists "1b" (
	"id"		integer not null,
	"subject"	text,
	"teacher"	text,
	primary key("id" autoincrement)
);
create table if not exists "1c" (
	"id"		integer not null,
	"subject"	text,
	"teacher"	text,
	primary key("id" autoincrement)
);
create table if not exists "1d" (
	"id"		integer not null,
	"subject"	text,
	"teacher"	text,
	primary key("id" autoincrement)
);
create table if not exists "1e" (
	"id"	integer not null,
	"subject"	text,
	"teacher"	text,
	primary key("id" autoincrement)
);
create table if not exists "1f" (
	"id"		integer not null,
	"subject"	text,
	"teacher"	text,
	primary key("id" autoincrement)
);
create table if not exists "2a" (
	"id"		integer not null,
	"subject"	text,
	"teacher"	text,
	primary key("id" autoincrement)
);
create table if not exists "2b" (
	"id"		integer not null,
	"subject"	text,
	"teacher"	text,
	primary key("id" autoincrement)
);
create table if not exists "2c" (
	"id"		integer not null,
	"subject"	text,
	"teacher"	text,
	primary key("id" autoincrement)
);
create table if not exists "2d" (
	"id"		integer not null,
	"subject"	text,
	"teacher"	text,
	primary key("id" autoincrement)
);
create table if not exists "2e" (
	"id"		integer not null,
	"subject"	text,
	"teacher"	text,
	primary key("id" autoincrement)
);
create table if not exists "3a" (
	"id"		integer not null,
	"subject"	text,
	"teacher"	text,
	primary key("id" autoincrement)
);
create table if not exists "3b" (
	"id"		integer not null,
	"subject"	text,
	"teacher"	text,
	primary key("id" autoincrement)
);
create table if not exists "3c" (
	"id"		integer not null,
	"subject"	text,
	"teacher"	text,
	primary key("id" autoincrement)
);
create table if not exists "3d" (
	"id"		integer not null,
	"subject"	text,
	"teacher"	text,
	primary key("id" autoincrement)
);
create table if not exists "3e" (
	"id"		integer not null,
	"subject"	text,
	"teacher"	text,
	primary key("id" autoincrement)
);
create table if not exists "3f" (
	"id"		integer not null,
	"subject"	text,
	"teacher"	text,
	primary key("id" autoincrement)
);
create table if not exists "4a" (
	"id"		integer not null,
	"subject"	text,
	"teacher"	text,
	primary key("id" autoincrement)
);
create table if not exists "4b" (
	"id"		integer not null,
	"subject"	text,
	"teacher"	text,
	primary key("id" autoincrement)
);
create table if not exists "4c" (
	"id"		integer not null,
	"subject"	text,
	"teacher"	text,
	primary key("id" autoincrement)
);
create table if not exists "4d" (
	"id"		integer not null,
	"subject"	text,
	"teacher"	text,
	primary key("id" autoincrement)
);
create table if not exists "4e" (
	"id"		integer not null,
	"subject"	text,
	"teacher"	text,
	primary key("id" autoincrement)
);
create table if not exists "4f" (
	"id"		integer not null,
	"subject"	text,
	"teacher"	text,
	primary key("id" autoincrement)
);

insert into "1a" values 
(null, 'administrando y promoviendo mi empresa', 'alejandro ancalaf ancalaf'),
(null, 'artes visuales', 'maría isabel valderrama díaz'),
(null, 'ciencias naturales', 'maría josé acuña espinoza'),
(null, 'empresario del siglo xxi', 'pedro acuña quijada'),
(null, 'educación física y salud', 'roberto sáez ramírez'),
(null, 'historia, geografía y ciencias sociales', 'génesis epullán arévalo'),
(null, 'idioma extranjero: inglés', 'maría josé arteaga azócar'),
(null, 'lengua y literatura', 'verónica romero mieres'),
(null, 'matemática', 'karla tripainao montecinos'),
(null, 'orientación', 'karla tripainao montecinos'),
(null, 'tecnología', 'bruno farfal reyes');

insert into "1b" values 
(null, 'administrando y promoviendo mi empresa', 'rodrigo gutiérrez manríquez'),
(null, 'artes visuales', 'maría isabel valderrama díaz'),
(null, 'ciencias naturales', 'maría josé acuña espinoza'),
(null, 'empresario del siglo xxi', 'alejandro ancalaf ancalaf'),
(null, 'educación física y salud', 'roberto sáez ramírez'),
(null, 'historia, geografía y ciencias sociales', 'génesis epullán arévalo'),
(null, 'idioma extranjero: inglés', 'maría josé arteaga azócar'),
(null, 'lengua y literatura', 'verónica romero mieres'),
(null, 'matemática', 'karla tripainao montecinos'),
(null, 'orientación', 'verónica romero mieres'),
(null, 'tecnología', 'bruno farfal reyes');

insert into "1c" values 
(null, 'administrando y promoviendo mi empresa', 'rodrigo gutiérrez manríquez'),
(null, 'artes visuales', 'maría isabel valderrama díaz'),
(null, 'ciencias naturales', 'maría josé acuña espinoza'),
(null, 'empresario del siglo xxi', 'alejandro ancalaf ancalaf'),
(null, 'educación física y salud', 'consuelo montes inostroza'),
(null, 'historia, geografía y ciencias sociales', 'génesis epullán arévalo'),
(null, 'idioma extranjero: inglés', 'maría josé arteaga azócar'),
(null, 'lengua y literatura', 'gabriela vega sáez'),
(null, 'matemática', 'gabriela bravo escalona'),
(null, 'orientación', 'gabriela bravo escalona'),
(null, 'tecnología', 'bruno farfal reyes');

insert into "1d" values 
(null, 'administrando y promoviendo mi empresa', 'pedro acuña quijada'),
(null, 'artes visuales', 'maría isabel valderrama díaz'),
(null, 'ciencias naturales', 'pedro acuña quijada'),
(null, 'empresario del siglo xxi', 'alejandro ancalaf ancalaf'),
(null, 'educación física y salud', 'roberto sáez ramírez'),
(null, 'historia, geografía y ciencias sociales', 'génesis epullán arévalo'),
(null, 'idioma extranjero: inglés', 'alexandra scholz rodríguez'),
(null, 'lengua y literatura', 'gabriela vega sáez'),
(null, 'matemática', 'gabriela bravo escalona'),
(null, 'orientación', 'pedro acuña quijada'),
(null, 'tecnología', 'bruno farfal reyes');

insert into "1e" values 
(null, 'administrando y promoviendo mi empresa', 'alejandro ancalaf ancalaf'),
(null, 'artes visuales', 'maría isabel valderrama díaz'),
(null, 'ciencias naturales', 'pedro acuña quijada'),
(null, 'empresario del siglo xxi', 'guillermo marín cortéz'),
(null, 'educación física y salud', 'consuelo montes inostroza'),
(null, 'historia, geografía y ciencias sociales', 'marcelo galleguillos higuera'),
(null, 'idioma extranjero: inglés', 'alexandra scholz rodríguez'),
(null, 'lengua y literatura', 'gabriela vega sáez'),
(null, 'matemática', 'karla tripainao montecinos'),
(null, 'orientación', 'guillermo marín cortéz'),
(null, 'tecnología', 'pedro acuña quijada');

insert into "1f" values 
(null, 'administrando y promoviendo mi empresa', 'rodrigo gutiérrez manríquez'),
(null, 'artes visuales', 'maría isabel valderrama díaz'),
(null, 'ciencias naturales', 'pedro acuña quijada'),
(null, 'empresario del siglo xxi', 'rodrigo gutiérrez manríquez'),
(null, 'educación física y salud', 'consuelo montes inostroza'),
(null, 'historia, geografía y ciencias sociales', 'emanuel araneda suazo'),
(null, 'idioma extranjero: inglés', 'alexandra scholz rodríguez'),
(null, 'lengua y literatura', 'clara morales cereceda'),
(null, 'matemática', 'karla tripainao montecinos'),
(null, 'orientación', 'emanuel araneda suazo'),
(null, 'tecnología', 'pedro acuña quijada');

insert into "2a" values 
(null, 'administrando y promoviendo mi empresa', 'rodrigo gutiérrez manríquez'),
(null, 'artes visuales', 'cristina león hernández'),
(null, 'ciencias naturales', 'maría josé acuña espinoza'),
(null, 'empresario del siglo xxi', 'rodrigo gutiérrez manríquez'),
(null, 'educación física y salud', 'roberto sáez ramírez'),
(null, 'historia, geografía y ciencias sociales', 'marcelo galleguillos higuera'),
(null, 'idioma extranjero: inglés', 'maría josé arteaga azócar'),
(null, 'lengua y literatura', 'verónica romero mieres'),
(null, 'matemática', 'tatiana cifuentes gajardo'),
(null, 'orientación', 'maría josé acuña espinoza'),
(null, 'tecnología', 'bruno farfal reyes');

insert into "2b" values 
(null, 'administrando y promoviendo mi empresa', 'betsabé zippel herrera'),
(null, 'artes visuales', 'maría isabel valderrama díaz'),
(null, 'ciencias naturales', 'maría josé acuña espinoza'),
(null, 'empresario del siglo xxi', 'alejandro ancalaf ancalaf'),
(null, 'educación física y salud', 'roberto sáez ramírez'),
(null, 'historia, geografía y ciencias sociales', 'génesis epullán arévalo'),
(null, 'idioma extranjero: inglés', 'maría josé arteaga azócar'),
(null, 'lengua y literatura', 'verónica romero mieres'),
(null, 'matemática', 'tatiana cifuentes gajardo'),
(null, 'orientación', 'roberto sáez ramírez'),
(null, 'tecnología', 'bruno farfal reyes');

insert into "2c" values 
(null, 'administrando y promoviendo mi empresa', 'rodrigo gutiérrez manríquez'),
(null, 'artes visuales', 'maría isabel valderrama díaz'),
(null, 'ciencias naturales', 'maría josé acuña espinoza'),
(null, 'empresario del siglo xxi', 'rodrigo gutiérrez manríquez'),
(null, 'educación física y salud', 'consuelo montes inostroza'),
(null, 'historia, geografía y ciencias sociales', 'génesis epullán arévalo'),
(null, 'idioma extranjero: inglés', 'maría josé arteaga azócar'),
(null, 'lengua y literatura', 'gabriela vega sáez'),
(null, 'matemática', 'tatiana cifuentes gajardo'),
(null, 'orientación', 'génesis epullán arévalo'),
(null, 'tecnología', 'bruno farfal reyes');

insert into "2d" values 
(null, 'administrando y promoviendo mi empresa', 'alejandro ancalaf ancalaf'),
(null, 'artes visuales', 'maría isabel valderrama díaz'),
(null, 'ciencias naturales', 'karin olivares chávez'),
(null, 'empresario del siglo xxi', 'pedro acuña quijada'),
(null, 'educación física y salud', 'roberto sáez ramírez'),
(null, 'historia, geografía y ciencias sociales', 'marcelo galleguillos higuera'),
(null, 'idioma extranjero: inglés', 'alexandra scholz rodríguez'),
(null, 'lengua y literatura', 'gabriela vega sáez'),
(null, 'matemática', 'gabriela bravo escalona'),
(null, 'orientación', 'gabriela vega sáez'),
(null, 'tecnología', 'bruno farfal reyes');

insert into "2e" values 
(null, 'administrando y promoviendo mi empresa', 'pedro acuña quijada'),
(null, 'artes visuales', 'maría isabel valderrama díaz'),
(null, 'ciencias naturales', 'karin olivares chávez'),
(null, 'empresario del siglo xxi', 'francisco orellana piña'),
(null, 'educación física y salud', 'consuelo montes inostroza'),
(null, 'historia, geografía y ciencias sociales', 'marcelo galleguillos higuera'),
(null, 'idioma extranjero: inglés', 'alexandra scholz rodríguez'),
(null, 'lengua y literatura', 'gabriela vega sáez'),
(null, 'matemática', 'gabriela bravo escalona'),
(null, 'orientación', 'karin olivares chávez'),
(null, 'tecnología', 'bruno farfal reyes');

insert into "3a" values 
(null, 'ciencias para la ciudadanía', 'karin olivares chávez'),
(null, 'educación ciudadana', 'génesis epullán arévalo'),
(null, 'educación física y salud', 'roberto sáez ramírez'),
(null, 'filosofía', 'marcelo galleguillos higuera'),
(null, 'idioma extranjero: inglés', 'alexandra scholz rodríguez'),
(null, 'lengua y literatura', 'gabriela vega sáez'),
(null, 'matemática', 'gabriela bravo escalona'),
(null, 'orientación', 'alexandra scholz rodríguez'),
(null, 'análisis de la experiencia en la empresa', 'alejandro ancalaf ancalaf'),
(null, 'atención de clientes', 'rodrigo gutiérrez manríquez'),
(null, 'cálculo y registro de remuneraciones', 'francisco orellana piña'),
(null, 'contabilización de operaciones comerciales', 'francisco orellana piña'),
(null, 'control y procesamiento de información contable', 'francisco orellana piña'),
(null, 'organización de oficinas', 'alejandro ancalaf ancalaf');

insert into "3b" values 
(null, 'ciencias para la ciudadanía', 'pedro acuña quijada'),
(null, 'educación ciudadana', 'génesis epullán arévalo'),
(null, 'educación física y salud', 'roberto sáez ramírez'),
(null, 'filosofía', 'marcelo galleguillos higuera'),
(null, 'idioma extranjero: inglés', 'maría josé arteaga azócar'),
(null, 'lengua y literatura', 'clara morales cereceda'),
(null, 'matemática', 'karla tripainao montecinos'),
(null, 'orientación', 'alejandro ancalaf ancalaf'),
(null, 'análisis de la experiencia en la empresa', 'sergio iturra merino'),
(null, 'aplicaciones informáticas para la gestión administrativa', 'luciano sepúlveda bravo'),
(null, 'atención de clientes', 'betsabé zippel herrera'),
(null, 'gestión comercial y tributaria', 'alejandro ancalaf ancalaf'),
(null, 'utilización de información contable', 'francisco orellana piña'),
(null, 'organización de oficinas', 'alejandro ancalaf ancalaf'),
(null, 'procesos administrativos', 'rodrigo gutiérrez manríquez');

insert into "3c" values 
(null, 'ciencias para la ciudadanía', 'karin olivares chávez'),
(null, 'educación ciudadana', 'génesis epullán arévalo'),
(null, 'educación física y salud', 'consuelo montes inostroza'),
(null, 'filosofía', 'marcelo galleguillos higuera'),
(null, 'idioma extranjero: inglés', 'maría josé arteaga azócar'),
(null, 'lengua y literatura', 'gabriela vega sáez'),
(null, 'matemática', 'gabriela bravo escalona'),
(null, 'orientación', 'luciano sepúlveda bravo'),
(null, 'instalación y configuración de equipos informáticos', 'luciano sepúlveda bravo'),
(null, 'programación y bases de datos', 'bruno farfal reyes'),
(null, 'sistemas operativos', 'luciano sepúlveda bravo'),
(null, 'soporte a usuarios y productividad', 'luciano sepúlveda bravo');

insert into "3d" values 
(null, 'ciencias para la ciudadanía', 'karin olivares chávez'),
(null, 'educación ciudadana', 'génesis epullán arévalo'),
(null, 'educación física y salud', 'consuelo montes inostroza'),
(null, 'filosofía', 'marcelo galleguillos higuera'),
(null, 'idioma extranjero: inglés', 'maría josé arteaga azócar'),
(null, 'lengua y literatura', 'clara morales cereceda'),
(null, 'matemática', 'zirta sobarzo caamaño'),
(null, 'orientación', 'consuelo montes inostroza'),
(null, 'análisis de la experiencia en la empresa', 'ekatelina gatica pereira'),
(null, 'impresión del producto gráfico', 'francisco ferrada cornejo'),
(null, 'materiales e insumos de la industria gráfica', 'francisco ferrada cornejo'),
(null, 'preparación de la máquina impresora', 'francisco ferrada cornejo'),
(null, 'verificación y preparación de archivos digitales', 'francisco ferrada cornejo');

insert into "3e" values 
(null, 'ciencias para la ciudadanía', 'pedro acuña quijada'),
(null, 'educación ciudadana', 'génesis epullán arévalo'),
(null, 'educación física y salud', 'roberto sáez ramírez'),
(null, 'filosofía', 'marcelo galleguillos higuera'),
(null, 'idioma extranjero: inglés', 'maría josé arteaga azócar'),
(null, 'lengua y literatura', 'gabriela vega sáez'),
(null, 'matemática', 'gabriela bravo escalona'),
(null, 'orientación', 'claudia galaz opitz'),
(null, 'análisis de la experiencia en la empresa', 'ekatelina gatica pereira'),
(null, 'impresión del producto gráfico', 'francisco ferrada cornejo'),
(null, 'materiales e insumos de la industria gráfica', 'claudia galaz opitz'),
(null, 'preparación de la máquina impresora', 'francisco ferrada cornejo'),
(null, 'verificación y preparación de archivos digitales', 'francisco ferrada cornejo');

insert into "4a" values 
(null, 'ciencias para la ciudadanía', 'karin olivares chávez'),
(null, 'educación ciudadana', 'emanuel araneda suazo'),
(null, 'educación física y salud', 'roberto sáez ramírez'),
(null, 'filosofía', 'marcelo galleguillos higuera'),
(null, 'idioma extranjero: inglés', 'alexandra scholz rodríguez'),
(null, 'lengua y literatura', 'clara morales cereceda'),
(null, 'matemática', 'tatiana cifuentes gajardo'),
(null, 'orientación', 'francisco orellana piña'),
(null, 'análisis de la experiencia en la empresa', 'alejandro ancalaf ancalaf'),
(null, 'emprendimiento y empleabilidad', 'alejandro ancalaf ancalaf'),
(null, 'atención de clientes', 'rodrigo gutiérrez manríquez'),
(null, 'cálculo y registro de impuestos', 'francisco orellana piña'),
(null, 'elaboración de informes contables', 'francisco orellana piña'),
(null, 'procesamiento de información contable-financiera', 'francisco orellana piña'),
(null, 'registro de operaciones de comercio nacional e internacional', 'francisco orellana piña');

insert into "4b" values 
(null, 'ciencias para la ciudadanía', 'karin olivares chávez'),
(null, 'educación ciudadana', 'emanuel araneda suazo'),
(null, 'educación física y salud', 'roberto sáez ramírez'),
(null, 'filosofía', 'marcelo galleguillos higuera'),
(null, 'idioma extranjero: inglés', 'maría josé arteaga azócar'),
(null, 'lengua y literatura', 'clara morales cereceda'),
(null, 'matemática', 'tatiana cifuentes gajardo'),
(null, 'orientación', 'alejandro ancalaf ancalaf'),
(null, 'análisis de la experiencia en la empresa', 'alejandro ancalaf ancalaf'),
(null, 'emprendimiento y empleabilidad', 'francisco orellana piña'),
(null, 'cálculo de remuneración, finiquitos y obligaciones laborales', 'francisco orellana piña'),
(null, 'desarrollo y bienestar del personal', 'alejandro ancalaf ancalaf'),
(null, 'dotación de personal', 'alejandro ancalaf ancalaf'),
(null, 'legislación laboral', 'alejandro ancalaf ancalaf');

insert into "4c" values 
(null, 'ciencias para la ciudadanía', 'karin olivares chávez'),
(null, 'educación ciudadana', 'marcelo galleguillos higuera'),
(null, 'educación física y salud', 'roberto sáez ramírez'),
(null, 'filosofía', 'marcelo galleguillos higuera'),
(null, 'idioma extranjero: inglés', 'maría josé arteaga azócar'),
(null, 'lengua y literatura', 'clara morales cereceda'),
(null, 'matemática', 'tatiana cifuentes gajardo'),
(null, 'orientación', 'maría josé arteaga azócar'),
(null, 'análisis de la experiencia en la empresa', 'luciano sepúlveda bravo'),
(null, 'emprendimiento y empleabilidad', 'bruno farfal reyes'),
(null, 'administración de base de datos', 'bruno farfal reyes'),
(null, 'desarrollo de aplicaciones web', 'bruno farfal reyes'),
(null, 'diseño de bases de datos relacionales', 'bruno farfal reyes'),
(null, 'programación orientada a objetos', 'bruno farfal reyes');

insert into "4d" values 
(null, 'ciencias para la ciudadanía', 'karin olivares chávez'),
(null, 'educación ciudadana', 'génesis epullán arévalo'),
(null, 'educación física y salud', 'consuelo montes inostroza'),
(null, 'filosofía', 'marcelo galleguillos higuera'),
(null, 'idioma extranjero: inglés', 'maría josé arteaga azócar'),
(null, 'lengua y literatura', 'clara morales cereceda'),
(null, 'matemática', 'tatiana cifuentes gajardo'),
(null, 'orientación', 'consuelo montes inostroza'),
(null, 'análisis de la experiencia en la empresa', 'ekatelina gatica pereira'),
(null, 'emprendimiento y empleabilidad', 'rodrigo gutiérrez manríquez'),
(null, 'encuadernación del producto impreso', 'francisco ferrada cornejo'),
(null, 'imposición de archivos y obtención de prueba de color', 'francisco ferrada cornejo'),
(null, 'post-impresión en embalajes y recubrimientos a impresos', 'claudia galaz opitz'),
(null, 'salida del archivo a matriz impresora e impresión digital', 'francisco ferrada cornejo');

insert into "4e" values 
(null, 'ciencias para la ciudadanía', 'karin olivares chávez'),
(null, 'educación ciudadana', 'génesis epullán arévalo'),
(null, 'educación física y salud', 'consuelo montes inostroza'),
(null, 'filosofía', 'marcelo galleguillos higuera'),
(null, 'idioma extranjero: inglés', 'alexandra scholz rodríguez'),
(null, 'lengua y literatura', 'clara morales cereceda'),
(null, 'matemática', 'karla tripainao montecinos'),
(null, 'orientación', 'francisco ferrada cornejo'),
(null, 'análisis de la experiencia en la empresa', 'ekatelina gatica pereira'),
(null, 'emprendimiento y empleabilidad', 'francisco orellana piña'),
(null, 'encuadernación del producto impreso', 'francisco ferrada cornejo'),
(null, 'imposición de archivos y obtención de prueba de color', 'francisco ferrada cornejo'),
(null, 'post-impresión en embalajes y recubrimientos a impresos', 'claudia galaz opitz'),
(null, 'salida del archivo a matriz impresora e impresión digital', 'francisco ferrada cornejo');
