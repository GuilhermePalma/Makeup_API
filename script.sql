DROP DATABASE IF EXISTS makeup_api;
CREATE DATABASE IF NOT EXISTS makeup_api;
USE makeup_api;

CREATE TABLE user(
    id_user INT NOT NULL AUTO_INCREMENT,
    nickname varchar(60) NOT NULL UNIQUE,
    email varchar(120) NOT NULL UNIQUE,
    name varchar(120) NOT NULL UNIQUE,
    password varchar(40) NOT NULL,
    idiom varchar(80) NOT NULL,
    theme_is_night BOOLEAN NOT NULL,
    PRIMARY KEY (id_user)
);

CREATE TABLE makeup(
    id_makeup INT NOT NULL AUTO_INCREMENT,
    name varchar(120) NOT NULL,
    id_brand INT NOT NULL,
    id_type INT NOT NULL,
    price varchar(10) NOT NULL,
    currency varchar(5) NOT NULL,
    description TEXT NOT NULL,
    url_image TEXT NOT NULL,
    PRIMARY KEY (id_makeup)
);

CREATE TABLE brand(
    id_brand INT NOT NULL AUTO_INCREMENT,
    name_brand varchar(70) NOT NULL,
    PRIMARY KEY (id_brand)
);

CREATE TABLE type(
    id_type INT NOT NULL AUTO_INCREMENT,
    name_type varchar(60) NOT NULL,
    PRIMARY KEY (id_type)
);

CREATE TABLE favorites(
    id_favorite INT NOT NULL AUTO_INCREMENT,
    id_user INT NOT NULL,
    id_makeup INT NOT NULL,
    PRIMARY KEY (id_favorite)
);

CREATE TABLE location(
    id_location INT NOT NULL AUTO_INCREMENT,
    id_user INT NOT NULL,
    id_country INT(80) NOT NULL,
    location_city_state INT NOT NULL,
    address varchar(255) NOT NULL,
    district varchar(255) NOT NULL,
    number INT NOT NULL,
    postal_code varchar(8) NOT NULL,
    correct_position BOOLEAN NOT NULL,
    PRIMARY KEY (id_location)
);

CREATE TABLE state_city(
    id_state_city INT NOT NULL AUTO_INCREMENT,
    name_state varchar(70) NOT NULL,
    name_city varchar(80) NOT NULL,
    PRIMARY KEY (id_state_city)
);

CREATE TABLE country(
    id_country INT NOT NULL AUTO_INCREMENT,
    country varchar(120) NOT NULL,
    PRIMARY KEY (id_country)
);

ALTER TABLE makeup ADD CONSTRAINT makeup_fk0 FOREIGN KEY (id_brand) REFERENCES brand(id_brand);
ALTER TABLE makeup ADD CONSTRAINT makeup_fk1 FOREIGN KEY (id_type) REFERENCES type(id_type);

ALTER TABLE favorites ADD CONSTRAINT favorites_fk0 FOREIGN KEY (id_user) REFERENCES user(id_user);
ALTER TABLE favorites ADD CONSTRAINT favorites_fk1 FOREIGN KEY (id_makeup) REFERENCES makeup(id_makeup);

ALTER TABLE location ADD CONSTRAINT location_fk0 FOREIGN KEY (id_user) REFERENCES user(id_user);
ALTER TABLE location ADD CONSTRAINT location_fk1 FOREIGN KEY (id_country) REFERENCES country(id_country);
ALTER TABLE location ADD CONSTRAINT location_fk2 FOREIGN KEY (location_city_state) REFERENCES state_city(id_state_city);