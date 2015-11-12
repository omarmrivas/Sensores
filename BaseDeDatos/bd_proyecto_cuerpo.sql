
-- phpMyAdmin SQL Dump
-- version 3.5.2.2
-- http://www.phpmyadmin.net
--
-- Servidor: localhost
-- Tiempo de generación: 06-11-2015 a las 13:42:01
-- Versión del servidor: 10.0.20-MariaDB
-- Versión de PHP: 5.2.17

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Base de datos: `u725774446_cons`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `areas`
--

CREATE TABLE IF NOT EXISTS `Areas` (
  `IdArea` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`IdArea`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci AUTO_INCREMENT=1;

-- ) ENGINE=MyISAM  DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

--
-- Volcado de datos para la tabla `areas`
--

INSERT INTO `Areas` (`IdArea`, `Nombre`) VALUES
(0, 'Alumno'),
(1, 'Profesor'),
(2, 'Administrativo'),
(3, 'Visitante'),
(4, 'Otro');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `especialidad`
--

CREATE TABLE IF NOT EXISTS `Especialidad` (
  `IdEspecialidad` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`IdEspecialidad`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci AUTO_INCREMENT=1;
-- ) ENGINE=MyISAM  DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

--
-- Volcado de datos para la tabla `especialidad`
--

INSERT INTO `Especialidad` (`IdEspecialidad`, `Nombre`) VALUES
(0, 'Cardiología'),
(1, 'Nutrición'),
(2, 'Pediatría'),
(3, 'Ginecología');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `exploracion_fisica`
--

CREATE TABLE IF NOT EXISTS `Exploracion_Fisica` (
  `IdExploracion` int(11) NOT NULL AUTO_INCREMENT,
  `IdPaciente` int(11) NOT NULL DEFAULT '0',
  `IdMedico` int(11) NOT NULL DEFAULT '0',
--  `IdEnfermeria` int(11) NOT NULL DEFAULT '0',
  `FechaEF` TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
--  `TAS` smallint(6) NOT NULL DEFAULT '0',
--  `TAD` smallint(6) NOT NULL DEFAULT '0',
--  `FrecCardiaca` smallint(6) NOT NULL DEFAULT '0',
--  `Temperatura` float NOT NULL DEFAULT '0',
--  `Talla` float NOT NULL DEFAULT '0',
--  `Peso` float NOT NULL DEFAULT '0',
  `CuadroClinico` text,
  `Diagnostico` text,
  `Tratamiento` text,
  `Referencia` text,
  `ProximaCita` date DEFAULT NULL,
  PRIMARY KEY (`IdExploracion`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci AUTO_INCREMENT=1;
-- ) ENGINE=MyISAM DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Medicos`
--

CREATE TABLE IF NOT EXISTS `Medicos` (
  `IdMedico` int(11) NOT NULL AUTO_INCREMENT,
  `ApellidoP` char(50) NOT NULL DEFAULT '0',
  `ApellidoM` char(50) NOT NULL DEFAULT '0',
  `Nombre` char(50) NOT NULL DEFAULT '0',
  `IdEspecialidad` tinyint(4) NOT NULL DEFAULT '0',
  `Consultorio` varchar(50) NOT NULL DEFAULT '0',
  `Cedula` varchar(50) NOT NULL DEFAULT '0',
  `FechaNac` date DEFAULT NULL,
  `Direccion` varchar(120) NOT NULL DEFAULT '0',
  `Telefono` varchar(10) NOT NULL DEFAULT '0',
  `Email` varchar(50) NOT NULL DEFAULT '0',
  `Password` varchar(50) NOT NULL DEFAULT '0',
  `TipoCuenta` int(11) DEFAULT NULL,
  PRIMARY KEY (`IdMedico`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci AUTO_INCREMENT=1;
-- ) ENGINE=MyISAM  DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

--
-- Volcado de datos para la tabla `Medicos`
--

INSERT INTO `Medicos` (`IdMedico`, `ApellidoP`, `ApellidoM`, `Nombre`, `IdEspecialidad`, `Consultorio`, `Cedula`, `FechaNac`, `Direccion`, `Telefono`, `Email`, `Password`, `TipoCuenta`) VALUES
(0, 'Arriaga', 'Velazquez', 'Pedro Damian', 2, 'A1', '1234567890', '1993-09-11', 'Cointziu 616', '1234567890', 'cesar@hotmail.com', 'torres', 1),
(1, 'Perez', 'Juache', 'Luis', 2, 'A23', '788996522', '1990-10-02', 'Insurgentes 14', '4443087332', 'juan.perez@hotmail.com', 'torres', 1),
(2, 'Reyes', 'Espinoza', 'Ana Maria', 3, 'A8', '789456123', '1985-10-01', 'Argentina 145', '4863310022', 'reyes.ana@hotmail.com', 'anita', 1),
(3, 'Pedroza', 'Pedrozq', 'Alonso', 2, 'A48', '4147885876', '2015-10-17', 'Juarez 1000', '422685555', 'alonso@hto.com', '7777', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `mpf`
--

CREATE TABLE IF NOT EXISTS `mpf` (
  `IdMetodo` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(50) NOT NULL DEFAULT '0',
  PRIMARY KEY (`IdMetodo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci AUTO_INCREMENT=1;
-- ) ENGINE=MyISAM  DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

--
-- Volcado de datos para la tabla `mpf`
--

INSERT INTO `mpf` (`IdMetodo`, `Nombre`) VALUES
(0, 'NO USA NI DESEA'),
(1, 'NO USA PERO SI DESEA'),
(2, 'MPX/EE INY'),
(3, 'MPX SOLA INY'),
(4, 'NORE+EE INY'),
(5, 'NORE (BIMESTRAO)'),
(6, 'DESO-EE VO'),
(7, 'LNG-EE VO'),
(8, 'IMPLANTE'),
(9, 'DIU'),
(10, 'PARCHE'),
(11, 'MIRENA'),
(12, 'OTB'),
(13, 'VASECTOMIA'),
(14, 'PRESERVATIVO');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Pacientes`
--

CREATE TABLE IF NOT EXISTS `Pacientes` (
  `IdPaciente` int(11) NOT NULL AUTO_INCREMENT,
  `Identificacion` varchar(50) NOT NULL DEFAULT '0',
  `ApellidoP` varchar(50) NOT NULL DEFAULT '0',
  `ApellidoM` varchar(50) NOT NULL DEFAULT '0',
  `Nombre` varchar(50) NOT NULL DEFAULT '0',
  `FechaNac` date DEFAULT NULL,
  `Direccion` varchar(120) NOT NULL DEFAULT '0',
  `Telefono` varchar(10) NOT NULL DEFAULT '0',
  `Email` varchar(50) NOT NULL DEFAULT '0',
  `NoParejas` int(11) NOT NULL DEFAULT '0',
  `IdArea` tinyint(4) NOT NULL DEFAULT '0',
  `Sexo` tinyint(4) NOT NULL DEFAULT '0',
  `IVSA` date DEFAULT NULL,
  `IdMPF` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`IdPaciente`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci AUTO_INCREMENT=1;
-- ) ENGINE=MyISAM  DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

--
-- Volcado de datos para la tabla `Pacientes`
--

INSERT INTO `Pacientes` (`IdPaciente`, `Identificacion`, `ApellidoP`, `ApellidoM`, `Nombre`, `FechaNac`, `Direccion`, `Telefono`, `Email`, `NoParejas`, `IdArea`, `Sexo`, `IVSA`, `IdMPF`) VALUES
(0, '110085', 'Torres', 'Reyes', 'Cesar Jonathan', '1993-05-16', 'Cointziu 610 A', '4443087332', 'cjtorresreyes@otmail.com', 0, 5, 1, '2007-05-16', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Variables`
--

CREATE TABLE IF NOT EXISTS `Variables` (
  `IdVariable` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(50) NOT NULL DEFAULT '0',
  PRIMARY KEY (`IdVariable`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci AUTO_INCREMENT=1;
-- ) ENGINE=MyISAM  DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

--
-- Volcado de datos para la tabla `Variables`
--

INSERT INTO `Variables` (`IdVariable`, `Nombre`) VALUES
(0, 'SPO2'),
(1, 'Tensión Arterial S'),
(2, 'Tensión Arterial D'),
(3, 'Frecuencia Cardiaca'),
(4, 'Frecuencia Respiratoria'),
(5, 'Temperatura'),
(6, 'Talla'),
(7, 'Peso'),
(8, 'Glucosa');



-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Mediciones`
--

CREATE TABLE IF NOT EXISTS `Mediciones` (
  `IdMedicion` int(11) NOT NULL AUTO_INCREMENT,
  `IdExploracion` int(11) NOT NULL,
  `IdVariable` int(11) NOT NULL,
  `Valor` int NOT NULL,
  `Tiempo` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`IdMedicion`),
  UNIQUE KEY `IdExploracion_IdVariable` (`IdExploracion`,`IdVariable`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_spanish_ci AUTO_INCREMENT=1;
-- ) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

--
-- Volcado de datos para la tabla `Mediciones`
--

INSERT INTO `Mediciones` (`IdMedicion`, `IdExploracion`, `IdVariable`, `Valor`, `Tiempo`) VALUES
(0, 1, 1, 97, '2015-05-25 17:26:30'),
(1, 1, 2, 85, '2015-05-25 17:26:30');


/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
