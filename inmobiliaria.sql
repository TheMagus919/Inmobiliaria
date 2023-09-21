-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 21-09-2023 a las 23:38:27
-- Versión del servidor: 10.4.21-MariaDB
-- Versión de PHP: 8.0.11

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliaria`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contratos`
--

CREATE TABLE `contratos` (
  `idContrato` int(11) NOT NULL,
  `idInquilino` int(11) NOT NULL,
  `idInmueble` int(11) NOT NULL,
  `fechaDesde` date NOT NULL,
  `fechaHasta` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `contratos`
--

INSERT INTO `contratos` (`idContrato`, `idInquilino`, `idInmueble`, `fechaDesde`, `fechaHasta`) VALUES
(1, 3, 2, '2023-08-28', '2023-09-03'),
(3, 3, 5, '2023-09-21', '2024-09-24');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmuebles`
--

CREATE TABLE `inmuebles` (
  `idInmueble` int(11) NOT NULL,
  `idPropietario` int(11) NOT NULL,
  `cantidadDeAmbientes` int(11) NOT NULL,
  `uso` enum('comercial','residencial') NOT NULL,
  `direccion` varchar(100) NOT NULL,
  `tipo` enum('local','deposito','casa','departamento') NOT NULL,
  `latitud` decimal(10,0) NOT NULL,
  `longitud` decimal(10,0) NOT NULL,
  `precio` decimal(10,0) NOT NULL,
  `disponible` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `inmuebles`
--

INSERT INTO `inmuebles` (`idInmueble`, `idPropietario`, `cantidadDeAmbientes`, `uso`, `direccion`, `tipo`, `latitud`, `longitud`, `precio`, `disponible`) VALUES
(1, 1, 2, 'residencial', 'barrio maria concepcion', 'local', '31', '12', '29999', 1),
(2, 2, 2, 'residencial', 'Pringles 1030', 'local', '22', '12', '59400', 1),
(4, 8, 7, 'comercial', 'las chacras 3021', 'departamento', '11', '13', '58888', 0),
(5, 9, 11, 'residencial', 'barrio belgrano 1202', 'casa', '5', '11', '100000', 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilinos`
--

CREATE TABLE `inquilinos` (
  `idInquilino` int(11) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `apellido` varchar(100) NOT NULL,
  `dni` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL,
  `telefono` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `inquilinos`
--

INSERT INTO `inquilinos` (`idInquilino`, `nombre`, `apellido`, `dni`, `email`, `telefono`) VALUES
(1, 'Pedro', 'Fonseca', '42675234', 'pedrof@gmail.com', '02556432455'),
(3, 'Mario', 'Kempes', '42675237', 'marianokempes@gmail.com', '2665142425');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `idPago` int(11) NOT NULL,
  `numeroDePago` int(11) NOT NULL,
  `fechaDePago` date NOT NULL,
  `importe` decimal(10,0) NOT NULL,
  `idContrato` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `pagos`
--

INSERT INTO `pagos` (`idPago`, `numeroDePago`, `fechaDePago`, `importe`, `idContrato`) VALUES
(1, 12, '2023-09-14', '12000', 1),
(3, 1, '2023-09-21', '2', 3);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietarios`
--

CREATE TABLE `propietarios` (
  `idPropietario` int(11) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `apellido` varchar(100) NOT NULL,
  `dni` varchar(100) NOT NULL,
  `telefono` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`idPropietario`, `nombre`, `apellido`, `dni`, `telefono`, `email`) VALUES
(1, 'Jorge Agustin', 'Barroso', '42278146', '02665258343', 'agustinbarroso919@gmail.com'),
(2, 'Pedro', 'Cruift', '23421456', '2222222222', 'marioc@gmail.com'),
(8, 'Alex', 'Lionel', '29452784', '2664645323', 'alex@gmail.com'),
(9, 'elias', 'gomez', '41412423', '225325246', 'elias@gmail.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `idUsuario` int(11) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `apellido` varchar(100) NOT NULL,
  `email` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL,
  `avatar` varchar(255) DEFAULT NULL,
  `rol` enum('empleado','administrador') NOT NULL DEFAULT 'empleado'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`idUsuario`, `nombre`, `apellido`, `email`, `password`, `avatar`, `rol`) VALUES
(2, 'carlo', 'frank', 'frankc@gmail.com', '0Od3ctO28xC/+R52IFE/dViLTUC+7P9636SJVUFkyK8=', '/Uploads\\avatar_2.png', 'administrador'),
(3, 'Toto', 'Vasco', 'eltoto@gmail.com', '6CoJujypOl0r7veNrE9nYn3cAX8t/OlK0EE3+miXUUA=', '/Uploads\\avatar_3.jpg', 'empleado'),
(4, 'Marcos', 'Pena', 'Marc@gmail.com', '9zIa+WH18BN9CDA2M+bIG4xnqe2YzgY5O2MH9YiWiks=', NULL, 'administrador'),
(6, 'marco', 'polo', 'polo@gmail.com', 'MuEgeclgAy0SR1apqZtfhlTWLQf4lAEZAU8SCKMt5Xo=', '/Uploads\\avatar_6.png', 'empleado');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD PRIMARY KEY (`idContrato`),
  ADD UNIQUE KEY `idInmueble_2` (`idInmueble`),
  ADD KEY `idInquilino` (`idInquilino`),
  ADD KEY `idInmueble` (`idInmueble`);

--
-- Indices de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD PRIMARY KEY (`idInmueble`),
  ADD UNIQUE KEY `direccion` (`direccion`),
  ADD KEY `idPropietario` (`idPropietario`);

--
-- Indices de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`idInquilino`),
  ADD UNIQUE KEY `dni` (`dni`),
  ADD UNIQUE KEY `email` (`email`);

--
-- Indices de la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD PRIMARY KEY (`idPago`),
  ADD KEY `idContrato` (`idContrato`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`idPropietario`),
  ADD UNIQUE KEY `dni` (`dni`),
  ADD UNIQUE KEY `email` (`email`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`idUsuario`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contratos`
--
ALTER TABLE `contratos`
  MODIFY `idContrato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `idInmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `idInquilino` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `pagos`
--
ALTER TABLE `pagos`
  MODIFY `idPago` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `idPropietario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `idUsuario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD CONSTRAINT `contratos_ibfk_1` FOREIGN KEY (`idInquilino`) REFERENCES `inquilinos` (`idInquilino`),
  ADD CONSTRAINT `contratos_ibfk_2` FOREIGN KEY (`idInmueble`) REFERENCES `inmuebles` (`idInmueble`);

--
-- Filtros para la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD CONSTRAINT `inmuebles_ibfk_1` FOREIGN KEY (`idPropietario`) REFERENCES `propietarios` (`idPropietario`);

--
-- Filtros para la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD CONSTRAINT `pagos_ibfk_1` FOREIGN KEY (`idContrato`) REFERENCES `contratos` (`idContrato`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
