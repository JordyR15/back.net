-- Procedimiento Almacenado para obtener todas las películas activas
CREATE OR REPLACE FUNCTION sp_get_all_peliculas()
RETURNS TABLE(id_pelicula INT, nombre VARCHAR, duracion INT, activo BOOLEAN)
AS $$
BEGIN
    RETURN QUERY 
    SELECT p.id_pelicula, p.nombre, p.duracion, p.activo
    FROM pelicula p
    WHERE p.activo = TRUE;
END;
$$ LANGUAGE plpgsql;
