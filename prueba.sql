--
-- PostgreSQL database dump
--

\restrict lnPD6FJk4YqKEQ2NIPulPS4qfvi3gc7iyQQb3XaA0ZsPYNx7hOwqhADWZxOLddu

-- Dumped from database version 18.0
-- Dumped by pg_dump version 18.0

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: pgcrypto; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS pgcrypto WITH SCHEMA public;


--
-- Name: EXTENSION pgcrypto; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION pgcrypto IS 'cryptographic functions';


--
-- Name: buscar_pelicula_por_nombre_y_sala(character varying, integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.buscar_pelicula_por_nombre_y_sala(nombre_buscado character varying, sala_id integer) RETURNS TABLE(id_pelicula integer, nombre_pelicula character varying, duracion integer, fecha_publicacion date, id_sala integer, nombre_sala character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY
    SELECT
        p.id_pelicula,
        p.nombre AS nombre_pelicula,
        p.duracion,
        psc.fecha_publicacion,
        sc.id_sala,
        sc.nombre AS nombre_sala
    FROM
        pelicula p
    INNER JOIN
        pelicula_salacine psc ON p.id_pelicula = psc.id_pelicula
    INNER JOIN
        sala_cine sc ON psc.id_sala_cine = sc.id_sala
    WHERE
        -- Condición 1: Si se envía un nombre, filtra por nombre. Si no, ignora esta condición.
        (nombre_buscado IS NULL OR p.nombre ILIKE '%' || nombre_buscado || '%')
        
        -- Condición 2: Si se envía un ID de sala, filtra por sala. Si no, ignora esta condición.
        AND (sala_id IS NULL OR sc.id_sala = sala_id);
END;
$$;


ALTER FUNCTION public.buscar_pelicula_por_nombre_y_sala(nombre_buscado character varying, sala_id integer) OWNER TO postgres;

--
-- Name: sp_get_all_peliculas(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_get_all_peliculas() RETURNS TABLE(id_pelicula integer, nombre character varying, duracion integer, activo boolean)
    LANGUAGE plpgsql
    AS $$
 BEGIN
     RETURN QUERY 
     SELECT p.id_pelicula, p.nombre, p.duracion, p.activo
     FROM pelicula p
     WHERE p.activo = TRUE;
 END;
 $$;


ALTER FUNCTION public.sp_get_all_peliculas() OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: pelicula; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.pelicula (
    id_pelicula integer NOT NULL,
    nombre character varying(255) NOT NULL,
    duracion integer,
    activo boolean DEFAULT true NOT NULL
);


ALTER TABLE public.pelicula OWNER TO postgres;

--
-- Name: pelicula_id_pelicula_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.pelicula_id_pelicula_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.pelicula_id_pelicula_seq OWNER TO postgres;

--
-- Name: pelicula_id_pelicula_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.pelicula_id_pelicula_seq OWNED BY public.pelicula.id_pelicula;


--
-- Name: pelicula_salacine; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.pelicula_salacine (
    id_pelicula_sala integer NOT NULL,
    id_pelicula integer NOT NULL,
    id_sala_cine integer NOT NULL,
    fecha_publicacion date,
    fecha_fin date
);


ALTER TABLE public.pelicula_salacine OWNER TO postgres;

--
-- Name: pelicula_salacine_id_pelicula_sala_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.pelicula_salacine_id_pelicula_sala_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.pelicula_salacine_id_pelicula_sala_seq OWNER TO postgres;

--
-- Name: pelicula_salacine_id_pelicula_sala_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.pelicula_salacine_id_pelicula_sala_seq OWNED BY public.pelicula_salacine.id_pelicula_sala;


--
-- Name: persons; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.persons (
    id bigint NOT NULL,
    first_name text NOT NULL,
    last_name text NOT NULL,
    email text NOT NULL,
    created_at timestamp with time zone DEFAULT now()
);


ALTER TABLE public.persons OWNER TO postgres;

--
-- Name: persons_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.persons ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.persons_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: roles; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.roles (
    id bigint NOT NULL,
    name text NOT NULL,
    activo boolean DEFAULT true NOT NULL
);


ALTER TABLE public.roles OWNER TO postgres;

--
-- Name: roles_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.roles ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.roles_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: sala_cine; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sala_cine (
    id_sala integer NOT NULL,
    nombre character varying(255) NOT NULL,
    estado integer DEFAULT 1,
    activo boolean DEFAULT true NOT NULL
);


ALTER TABLE public.sala_cine OWNER TO postgres;

--
-- Name: sala_cine_id_sala_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.sala_cine_id_sala_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.sala_cine_id_sala_seq OWNER TO postgres;

--
-- Name: sala_cine_id_sala_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.sala_cine_id_sala_seq OWNED BY public.sala_cine.id_sala;


--
-- Name: sessions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sessions (
    id bigint NOT NULL,
    created_at timestamp without time zone,
    expires_at timestamp without time zone NOT NULL,
    id_session text NOT NULL,
    session_active boolean,
    user_id bigint
);


ALTER TABLE public.sessions OWNER TO postgres;

--
-- Name: sessions_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.sessions_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.sessions_id_seq OWNER TO postgres;

--
-- Name: sessions_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.sessions_id_seq OWNED BY public.sessions.id;


--
-- Name: user_roles; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.user_roles (
    user_id bigint NOT NULL,
    role_id bigint NOT NULL
);


ALTER TABLE public.user_roles OWNER TO postgres;

--
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    id bigint NOT NULL,
    username text NOT NULL,
    password_hash text NOT NULL,
    created_at timestamp with time zone DEFAULT now(),
    updated_at timestamp with time zone DEFAULT now(),
    person_id bigint,
    reset_token character varying(255),
    reset_token_expiry timestamp without time zone,
    activo boolean DEFAULT true NOT NULL
);


ALTER TABLE public.users OWNER TO postgres;

--
-- Name: users_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.users ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.users_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: pelicula id_pelicula; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.pelicula ALTER COLUMN id_pelicula SET DEFAULT nextval('public.pelicula_id_pelicula_seq'::regclass);


--
-- Name: pelicula_salacine id_pelicula_sala; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.pelicula_salacine ALTER COLUMN id_pelicula_sala SET DEFAULT nextval('public.pelicula_salacine_id_pelicula_sala_seq'::regclass);


--
-- Name: sala_cine id_sala; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sala_cine ALTER COLUMN id_sala SET DEFAULT nextval('public.sala_cine_id_sala_seq'::regclass);


--
-- Name: sessions id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sessions ALTER COLUMN id SET DEFAULT nextval('public.sessions_id_seq'::regclass);


--
-- Data for Name: pelicula; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.pelicula (id_pelicula, nombre, duracion, activo) FROM stdin;
1	El Padrino: Parte II	202	t
5	Tiburon	220	t
7	El viaje 2	500	t
8	The Substance 2	90	f
9	Superman 	95	t
10	Flash	145	f
11	Man of steel	75	t
4	Batman regresa	287	t
12	Bingo 2	125	f
13	Balto	95	t
14	God of War 2	250	t
15	Deluxe 2	90	f
16	La anaconda cabezona	45	f
17	Pokemon	45	t
18	Arcane	145	t
20	Pokemon: Gen 2	155	t
21	Mori: La venganza	120	t
19	Dos t2	59	f
22	El Origen de las Pruebas (Versión Extendida)	180	f
\.


--
-- Data for Name: pelicula_salacine; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.pelicula_salacine (id_pelicula_sala, id_pelicula, id_sala_cine, fecha_publicacion, fecha_fin) FROM stdin;
2	4	1	2025-09-21	2025-09-14
3	4	2	2025-10-01	2025-10-12
4	1	1	2025-10-05	2025-10-12
5	5	4	2025-09-18	2025-09-28
6	7	3	2025-09-30	2025-10-12
7	4	3	2025-09-30	2025-10-19
8	7	1	2025-10-01	2025-10-15
9	11	8	2025-10-03	2025-10-19
10	5	6	2025-10-03	2025-10-11
11	13	3	2025-10-03	2025-10-12
12	14	3	2025-10-03	2025-10-26
1	1	2	2025-10-11	2025-10-18
13	1	4	2025-10-18	2025-10-19
14	1	8	2025-10-05	2025-10-11
15	7	6	2025-10-09	2025-10-29
16	7	6	2025-10-09	2025-10-29
17	11	7	2025-10-04	2025-10-18
18	17	8	2025-11-03	2025-11-09
19	17	6	2025-11-03	2025-11-09
20	18	8	2025-11-03	2025-11-09
21	17	8	2025-11-03	2025-11-09
22	7	8	2025-11-03	2025-11-09
23	18	11	2025-11-03	2025-11-09
24	4	11	2025-11-03	2025-11-09
25	13	11	2025-11-03	2025-11-09
26	11	11	2025-11-03	2025-11-09
27	7	11	2025-11-03	2025-11-09
28	18	3	2025-11-03	2025-11-09
29	11	11	2025-11-03	2025-11-09
30	19	12	2025-11-03	2025-11-09
31	19	12	2025-11-03	2025-11-09
32	21	13	2025-11-03	2025-11-09
33	21	4	2025-11-03	2025-11-05
\.


--
-- Data for Name: persons; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.persons (id, first_name, last_name, email, created_at) FROM stdin;
3	Jordy	Rivas	jordyrivas15@gmail.com	2025-09-27 11:40:33.651823-05
4	John	Doe	john.doe@example.com	2025-09-28 13:42:00.298241-05
6	Jordy	Rivas	jordyrivas13@gmail.com	2025-09-29 02:35:25.96243-05
7	Juan	Luis	juanl@gmail.com	2025-09-29 02:48:03.613225-05
10	Juan	Luis	juanl01@gmail.com	2025-09-29 02:56:54.393989-05
11	Lola	Indigo	lolalolita@gmail.com	2025-09-30 13:35:26.992836-05
12	Pepe	Perez	elpepe@gmail.com	2025-09-30 23:23:11.856998-05
13	pandrango	pandrango	pandrango@gmail.com	2025-10-01 06:44:25.148605-05
14	Jhonny	Adriel	jadriel@gmail.com	2025-10-01 07:25:03.226263-05
\.


--
-- Data for Name: roles; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.roles (id, name, activo) FROM stdin;
1	Admin	t
2	Cliente	t
\.


--
-- Data for Name: sala_cine; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.sala_cine (id_sala, nombre, estado, activo) FROM stdin;
3	Gilo	1	t
4	Lora	1	t
5	Sala Premium	1	f
6	Lolera X	1	t
7	Hola	1	t
1	Loj	1	f
8	Mendigo	1	t
9	Pepio	1	f
10	Mar Brava costera	1	f
11	P-Sol	1	t
12	Fitrol 22	1	t
13	Locke	1	t
2	Imax pro max	1	t
14	Premium 25HD	1	f
\.


--
-- Data for Name: sessions; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.sessions (id, created_at, expires_at, id_session, session_active, user_id) FROM stdin;
\.


--
-- Data for Name: user_roles; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.user_roles (user_id, role_id) FROM stdin;
2	1
3	2
4	1
5	2
7	2
8	1
9	2
10	2
\.


--
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.users (id, username, password_hash, created_at, updated_at, person_id, reset_token, reset_token_expiry, activo) FROM stdin;
3	testuser	$2a$10$tBk03TDm629gfszggldqvOkgCpev/cFqqckKpzziApgSjKiCJLgj6	2025-09-28 13:42:00.555391-05	2025-09-28 13:42:00.297941-05	4	\N	\N	t
2	admin	$2a$10$jheO2LolvURZfoCN/SSgsenGzSESHYnJNpB0aYPB4yXzF8XHYIxte	2025-09-27 11:40:52.915835-05	2025-09-27 11:40:52.915835-05	3	f6a7174e-1ee9-4263-94d4-6c465aaad0ee	2025-09-29 03:00:44.733298	t
4	ryu	$2a$10$i4n7X2B2muDkQeUgEEBZB.x3xmxi8rkIFcIWPZYsW2RoxkSScnNOC	2025-09-29 02:35:26.199428-05	2025-09-29 02:35:25.962522-05	6	\N	\N	t
5	juanl	$2a$10$gLgRgY3yusvgr/8UqIT6muAp9j8jG6Eh8DPIh43BAmpw8LQMkJ37y	2025-09-29 02:48:03.790216-05	2025-09-29 02:48:03.611826-05	7	\N	\N	t
7	lolalita	$2a$10$UasItDMEzcr8D5stnNxzoegO2YdY9Xy9zH9XGGw1kT4ivwfiryBKq	2025-09-30 13:35:27.248838-05	2025-09-30 13:35:26.981046-05	11	\N	\N	t
8	Pepe	$2a$10$ILxGTcoVhKNqi.sO.dcK6uGRPiHss5slcNlCNMpmN6Rg4IN04YIKe	2025-09-30 23:23:12.178994-05	2025-09-30 23:23:11.840056-05	12	\N	\N	t
6	juan01	$2a$10$t6SF5S1kQAWhU2TmqT42YejAwIHzYOkAuUwAXfEPjs0PisqjoGGru	2025-09-29 02:56:54.613986-05	2025-09-29 02:56:54.393735-05	10	\N	\N	f
9	pandrango	$2a$10$2iAEA7H3lYwlZNkSplzSbu7qY.YuxK16/vs/4nFtMVa15UlZN7T/6	2025-10-01 06:44:25.457606-05	2025-10-01 06:44:25.141489-05	13	\N	\N	t
10	jadriel	$2a$10$HYoKeRKlT103HrL14fAExuVVjxcGeHPTRJCwmSvLR4pHeF/sREus6	2025-10-01 07:25:03.438263-05	2025-10-01 07:25:03.215098-05	14	\N	\N	t
\.


--
-- Name: pelicula_id_pelicula_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.pelicula_id_pelicula_seq', 22, true);


--
-- Name: pelicula_salacine_id_pelicula_sala_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.pelicula_salacine_id_pelicula_sala_seq', 33, true);


--
-- Name: persons_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.persons_id_seq', 14, true);


--
-- Name: roles_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.roles_id_seq', 2, true);


--
-- Name: sala_cine_id_sala_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.sala_cine_id_sala_seq', 14, true);


--
-- Name: sessions_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.sessions_id_seq', 1, false);


--
-- Name: users_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.users_id_seq', 10, true);


--
-- Name: pelicula pelicula_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.pelicula
    ADD CONSTRAINT pelicula_pkey PRIMARY KEY (id_pelicula);


--
-- Name: pelicula_salacine pelicula_salacine_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.pelicula_salacine
    ADD CONSTRAINT pelicula_salacine_pkey PRIMARY KEY (id_pelicula_sala);


--
-- Name: persons persons_email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.persons
    ADD CONSTRAINT persons_email_key UNIQUE (email);


--
-- Name: persons persons_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.persons
    ADD CONSTRAINT persons_pkey PRIMARY KEY (id);


--
-- Name: roles roles_name_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.roles
    ADD CONSTRAINT roles_name_key UNIQUE (name);


--
-- Name: roles roles_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.roles
    ADD CONSTRAINT roles_pkey PRIMARY KEY (id);


--
-- Name: sala_cine sala_cine_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sala_cine
    ADD CONSTRAINT sala_cine_pkey PRIMARY KEY (id_sala);


--
-- Name: sessions sessions_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sessions
    ADD CONSTRAINT sessions_pkey PRIMARY KEY (id);


--
-- Name: users ukr43af9ap4edm43mmtq01oddj6; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT ukr43af9ap4edm43mmtq01oddj6 UNIQUE (username);


--
-- Name: user_roles user_roles_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_roles
    ADD CONSTRAINT user_roles_pkey PRIMARY KEY (user_id, role_id);


--
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (id);


--
-- Name: users users_username_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_username_key UNIQUE (username);


--
-- Name: pelicula_salacine fk_pelicula; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.pelicula_salacine
    ADD CONSTRAINT fk_pelicula FOREIGN KEY (id_pelicula) REFERENCES public.pelicula(id_pelicula);


--
-- Name: pelicula_salacine fk_sala_cine; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.pelicula_salacine
    ADD CONSTRAINT fk_sala_cine FOREIGN KEY (id_sala_cine) REFERENCES public.sala_cine(id_sala);


--
-- Name: sessions fkruie73rneumyyd1bgo6qw8vjt; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sessions
    ADD CONSTRAINT fkruie73rneumyyd1bgo6qw8vjt FOREIGN KEY (user_id) REFERENCES public.users(id) ON DELETE CASCADE;


--
-- Name: user_roles user_roles_role_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_roles
    ADD CONSTRAINT user_roles_role_id_fkey FOREIGN KEY (role_id) REFERENCES public.roles(id) ON DELETE CASCADE;


--
-- Name: user_roles user_roles_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_roles
    ADD CONSTRAINT user_roles_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(id) ON DELETE CASCADE;


--
-- Name: users users_person_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_person_id_fkey FOREIGN KEY (person_id) REFERENCES public.persons(id) ON DELETE CASCADE;


--
-- PostgreSQL database dump complete
--

\unrestrict lnPD6FJk4YqKEQ2NIPulPS4qfvi3gc7iyQQb3XaA0ZsPYNx7hOwqhADWZxOLddu

