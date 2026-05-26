-- Inserta datos de prueba solo si no existen por Nombre
INSERT INTO "Products" ("Nombre", "Precio", "Stock", "FechaCreacion")
SELECT 'Laptop Lenovo ThinkPad', 4200.00, 8, NOW()
WHERE NOT EXISTS (SELECT 1 FROM "Products" WHERE "Nombre" = 'Laptop Lenovo ThinkPad');

INSERT INTO "Products" ("Nombre", "Precio", "Stock", "FechaCreacion")
SELECT 'Mouse Logitech M185', 55.90, 120, NOW()
WHERE NOT EXISTS (SELECT 1 FROM "Products" WHERE "Nombre" = 'Mouse Logitech M185');

INSERT INTO "Products" ("Nombre", "Precio", "Stock", "FechaCreacion")
SELECT 'Teclado Mecánico Redragon', 189.50, 40, NOW()
WHERE NOT EXISTS (SELECT 1 FROM "Products" WHERE "Nombre" = 'Teclado Mecánico Redragon');

INSERT INTO "Products" ("Nombre", "Precio", "Stock", "FechaCreacion")
SELECT 'Monitor Samsung 24"', 799.99, 25, NOW()
WHERE NOT EXISTS (SELECT 1 FROM "Products" WHERE "Nombre" = 'Monitor Samsung 24"');

INSERT INTO "Products" ("Nombre", "Precio", "Stock", "FechaCreacion")
SELECT 'Disco SSD Kingston 1TB', 350.00, 60, NOW()
WHERE NOT EXISTS (SELECT 1 FROM "Products" WHERE "Nombre" = 'Disco SSD Kingston 1TB');
