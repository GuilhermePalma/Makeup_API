DROP DATABASE IF EXISTS makeup_api;
CREATE DATABASE makeup_api;

use makeup_api;

CREATE TABLE `user` (
	`id_user` INT NOT NULL AUTO_INCREMENT,
	`nickname` varchar(60) NOT NULL UNIQUE,
	`email` varchar(120) NOT NULL UNIQUE,
	`name` varchar(120) NOT NULL UNIQUE,
	`password` varchar(40) NOT NULL,
	`idiom` varchar(80) NOT NULL,
	`theme_is_night` BOOLEAN NOT NULL,
	PRIMARY KEY (`id_user`)
);

CREATE TABLE `makeup` (
	`id_makeup` INT NOT NULL AUTO_INCREMENT,
	`name` varchar(120) NOT NULL,
	`id_brand` INT NOT NULL,
	`id_type` INT NOT NULL,
	PRIMARY KEY (`id_makeup`)
);

CREATE TABLE `brand` (
	`id_brand` INT NOT NULL AUTO_INCREMENT,
	`name_brand` varchar(70) NOT NULL,
	PRIMARY KEY (`id_brand`)
);

CREATE TABLE `type` (
	`id_type` INT NOT NULL AUTO_INCREMENT,
	`name_type` varchar(60) NOT NULL,
	PRIMARY KEY (`id_type`)
);

CREATE TABLE `favorites` (
	`id_favorite` INT NOT NULL AUTO_INCREMENT,
	`id_user` INT NOT NULL,
	`id_makeup` INT NOT NULL,
	PRIMARY KEY (`id_favorite`)
);

CREATE TABLE `location` (
	`id_location` INT NOT NULL AUTO_INCREMENT,
	`id_user` INT NOT NULL,
	`id_country` INT(80) NOT NULL,
	`location_city_state` INT NOT NULL,
	`address` varchar(255) NOT NULL,
	`district` varchar(255) NOT NULL,
	`number` INT NOT NULL,
	`postal_code` varchar(8) NOT NULL,
	`correct_position` BOOLEAN NOT NULL,
	PRIMARY KEY (`id_location`)
);

CREATE TABLE `state_city` (
	`id_state_city` INT NOT NULL AUTO_INCREMENT,
	`name_state` varchar(70) NOT NULL,
	`name_city` varchar(80) NOT NULL,
	PRIMARY KEY (`id_state_city`)
);

CREATE TABLE `country` (
	`id_country` INT NOT NULL AUTO_INCREMENT,
	`country` varchar(120) NOT NULL,
	PRIMARY KEY (`id_country`)
);

ALTER TABLE `makeup` ADD CONSTRAINT `makeup_fk0` FOREIGN KEY (`id_brand`) REFERENCES `brand`(`id_brand`);

ALTER TABLE `makeup` ADD CONSTRAINT `makeup_fk1` FOREIGN KEY (`id_type`) REFERENCES `type`(`id_type`);

ALTER TABLE `favorites` ADD CONSTRAINT `favorites_fk0` FOREIGN KEY (`id_user`) REFERENCES `user`(`id_user`);

ALTER TABLE `favorites` ADD CONSTRAINT `favorites_fk1` FOREIGN KEY (`id_makeup`) REFERENCES `makeup`(`id_makeup`);

ALTER TABLE `location` ADD CONSTRAINT `location_fk0` FOREIGN KEY (`id_user`) REFERENCES `user`(`id_user`);

ALTER TABLE `location` ADD CONSTRAINT `location_fk1` FOREIGN KEY (`id_country`) REFERENCES `country`(`id_country`);

ALTER TABLE `location` ADD CONSTRAINT `location_fk2` FOREIGN KEY (`location_city_state`) REFERENCES `state_city`(`id_state_city`);

INSERT INTO `user` (`id_user`, `nickname`, `email`, `name`, `password`, `idiom`, `theme_is_night`) VALUES
(1, 'teste', 'fasfasdasda', 'fasasdasfas', 'fasfa', 'Ingles', 0),
(2, 'dasndkas', 'testeedasda', 'dasdas', 'fjaskfas', 'Ingles', 0),
(3, 'dasnaddkas', 'testeedadassda', 'daadsdas', 'fjaskfdasdaas', 'Ingles', 0),
(4, 'user_test', 'emailtest@gmeil.com', 'Name Test', 'accountforteste', 'Portugues', 0),
(5, 'luis', 'luis@gmail', 'Luis', 'admin', 'Ingles', 1);

INSERT INTO type(id_type, name_type) VALUES
(1,"Blush"),
(2, "Pulyte"),
(3, "Objetius"),
(4, "Maose Uls");

INSERT INTO brand(id_brand, name_brand) VALUES
(1,"polien"),
(2, "moliru"),
(3, "molse");

INSERT INTO makeup(id_makeup, name, id_brand, id_type) VALUES
(1,"Lorem Ipsulin", 1,1),
(2,"Kilap Guti", 2,2),
(3,"Lopar Tuni", 3,3),
(4,"Luja Noshua", 3,4);

INSERT INTO favorites(id_favorite, id_user, id_makeup) VALUES
(1,1,1),
(2,2,2),
(3,2,3),
(4,3,4);

use makeup_api;
SELECT * FROM user;