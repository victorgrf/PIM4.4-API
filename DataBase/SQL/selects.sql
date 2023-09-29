SELECT
    a.*,
    p.*
FROM aluno a
JOIN pessoa p ON p.id = a.id
WHERE a.id = 10010;

ALTER TABLE pessoa ADD COLUMN senhaAlterada BOOLEAN NOT NULL;