--
-- PostgreSQL database dump
--

-- Dumped from database version 10.4
-- Dumped by pg_dump version 10.4

-- Started on 2018-10-02 13:59:06

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET client_min_messages = warning;
SET row_security = off;

DROP DATABASE musiclibrary;
--
-- TOC entry 2184 (class 1262 OID 16390)
-- Name: musiclibrary; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE musiclibrary WITH TEMPLATE = template0 ENCODING = 'UTF8' LC_COLLATE = 'English_United States.1252' LC_CTYPE = 'English_United States.1252';


ALTER DATABASE musiclibrary OWNER TO postgres;

\connect musiclibrary

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 1 (class 3079 OID 12278)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 2187 (class 0 OID 0)
-- Dependencies: 1
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


--
-- TOC entry 204 (class 1255 OID 16750)
-- Name: info_from_work_version_id(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.info_from_work_version_id(wvid integer) RETURNS TABLE(g_name character varying, a_name character varying, w_name character varying, wv_name character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN

RETURN QUERY select genre.genre_name, artist.artist_name, work.work_name, work_version.work_version_name
from genre, artist, work, work_version
	where work_version.work_id = work.work_id and 
	work.artist_id = artist.artist_id and
	artist.genre_id = genre.genre_id and
	work_version.work_version_id = wvid;
END; $$;


ALTER FUNCTION public.info_from_work_version_id(wvid integer) OWNER TO postgres;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 199 (class 1259 OID 16446)
-- Name: artist; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.artist (
    artist_id integer NOT NULL,
    genre_id integer NOT NULL,
    artist_name character varying NOT NULL
);


ALTER TABLE public.artist OWNER TO postgres;

--
-- TOC entry 198 (class 1259 OID 16444)
-- Name: artist_artist_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.artist ALTER COLUMN artist_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.artist_artist_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 197 (class 1259 OID 16411)
-- Name: genre; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.genre (
    genre_id integer NOT NULL,
    genre_name character varying NOT NULL
);


ALTER TABLE public.genre OWNER TO postgres;

--
-- TOC entry 196 (class 1259 OID 16409)
-- Name: genre_genre_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.genre ALTER COLUMN genre_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.genre_genre_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 201 (class 1259 OID 16497)
-- Name: work; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.work (
    work_id integer NOT NULL,
    artist_id integer NOT NULL,
    work_name character varying NOT NULL
);


ALTER TABLE public.work OWNER TO postgres;

--
-- TOC entry 203 (class 1259 OID 16562)
-- Name: work_version; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.work_version (
    work_version_id integer NOT NULL,
    work_id integer NOT NULL,
    work_version_name character varying NOT NULL,
    lossless boolean NOT NULL
);


ALTER TABLE public.work_version OWNER TO postgres;

--
-- TOC entry 202 (class 1259 OID 16560)
-- Name: work_version_work_version_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.work_version ALTER COLUMN work_version_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.work_version_work_version_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 200 (class 1259 OID 16495)
-- Name: work_work_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.work ALTER COLUMN work_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.work_work_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 2050 (class 2606 OID 16453)
-- Name: artist artist_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.artist
    ADD CONSTRAINT artist_pkey PRIMARY KEY (artist_id);


--
-- TOC entry 2048 (class 2606 OID 16418)
-- Name: genre genre_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.genre
    ADD CONSTRAINT genre_pkey PRIMARY KEY (genre_id);


--
-- TOC entry 2052 (class 2606 OID 16504)
-- Name: work work_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.work
    ADD CONSTRAINT work_pkey PRIMARY KEY (work_id);


--
-- TOC entry 2054 (class 2606 OID 16569)
-- Name: work_version work_version_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.work_version
    ADD CONSTRAINT work_version_pkey PRIMARY KEY (work_version_id);


--
-- TOC entry 2055 (class 2606 OID 16454)
-- Name: artist artist_genre_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.artist
    ADD CONSTRAINT artist_genre_id_fkey FOREIGN KEY (genre_id) REFERENCES public.genre(genre_id) ON DELETE CASCADE;


--
-- TOC entry 2056 (class 2606 OID 16505)
-- Name: work work_artist_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.work
    ADD CONSTRAINT work_artist_id_fkey FOREIGN KEY (artist_id) REFERENCES public.artist(artist_id) ON DELETE CASCADE;


--
-- TOC entry 2057 (class 2606 OID 16570)
-- Name: work_version work_version_work_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.work_version
    ADD CONSTRAINT work_version_work_id_fkey FOREIGN KEY (work_id) REFERENCES public.work(work_id) ON DELETE CASCADE;


--
-- TOC entry 2186 (class 0 OID 0)
-- Dependencies: 5
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: postgres
--

GRANT ALL ON SCHEMA public TO PUBLIC;


-- Completed on 2018-10-02 13:59:07

--
-- PostgreSQL database dump complete
--

