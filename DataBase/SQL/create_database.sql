CREATE DATABASE IF NOT EXISTS db_a9f943_pim;

CREATE TABLE IF NOT EXISTS db_a9f943_pim.pessoa (
	id       INT          NOT NULL UNIQUE AUTO_INCREMENT,
    senha    VARCHAR(255)  NOT NULL,
    senhaAlterada BOOLEAN NOT NULL,
    nome     VARCHAR(255) NOT NULL,
    cpf      BIGINT       NOT NULL UNIQUE,
    rg       BIGINT       NOT NULL UNIQUE,
    email    VARCHAR(255) NOT NULL UNIQUE,
    telefone BIGINT,
    cargo    VARCHAR(10)  NOT NULL, #ENUM('AnalistaRH','Secretario','Professor','Aluno'),
    
    CONSTRAINT PK_pessoa PRIMARY KEY (id)
) AUTO_INCREMENT = 10001;

CREATE TABLE IF NOT EXISTS db_a9f943_pim.analistarh (
	id INT NOT NULL UNIQUE,
    
    CONSTRAINT PK_analistarh        PRIMARY KEY(id),
    CONSTRAINT FK_analistarh_pessoa FOREIGN KEY(id) REFERENCES db_a9f943_pim.pessoa(id)
);

CREATE TABLE IF NOT EXISTS db_a9f943_pim.secretario (
	id INT NOT NULL UNIQUE,
    
    CONSTRAINT PK_secretario        PRIMARY KEY(id),
    CONSTRAINT FK_secretario_pessoa FOREIGN KEY(id) REFERENCES db_a9f943_pim.pessoa(id)
);

CREATE TABLE IF NOT EXISTS db_a9f943_pim.professor (
	id INT NOT NULL UNIQUE,
    
    CONSTRAINT PK_professor        PRIMARY KEY(id),
    CONSTRAINT FK_professor_pessoa FOREIGN KEY(id) REFERENCES db_a9f943_pim.pessoa(id)
);

CREATE TABLE IF NOT EXISTS db_a9f943_pim.aluno (
	id INT NOT NULL UNIQUE,
    
    CONSTRAINT PK_aluno        PRIMARY KEY(id),
    CONSTRAINT FK_aluno_pessoa FOREIGN KEY(id) REFERENCES db_a9f943_pim.pessoa(id)
);

CREATE TABLE IF NOT EXISTS db_a9f943_pim.disciplina (
	id   INT NOT      NULL UNIQUE AUTO_INCREMENT,
    nome VARCHAR(255) NOT NULL UNIQUE,
    
    CONSTRAINT PK_disciplina PRIMARY KEY(id)
) AUTO_INCREMENT = 10001;

CREATE TABLE IF NOT EXISTS db_a9f943_pim.curso (
	id           INT          NOT NULL UNIQUE AUTO_INCREMENT,
    nome         VARCHAR(255) NOT NULL UNIQUE,
    cargaHoraria INT          NOT NULL,
    aulasTotais  INT          NOT NULL,
    
    CONSTRAINT PK_curso PRIMARY KEY(id)
) AUTO_INCREMENT = 10001;

CREATE TABLE IF NOT EXISTS db_a9f943_pim.turma (
	id           INT          NOT NULL UNIQUE AUTO_INCREMENT,
    idCurso      INT          NOT NULL,
    nome         VARCHAR(255) NOT NULL UNIQUE,
    
    CONSTRAINT PK_turma PRIMARY KEY(id),
    CONSTRAINT FK_turma_curso FOREIGN KEY(idCurso) REFERENCES db_a9f943_pim.curso(id)
) AUTO_INCREMENT = 10001;

CREATE TABLE IF NOT EXISTS db_a9f943_pim.disciplinaMinistrada (
	id           INT NOT NULL UNIQUE AUTO_INCREMENT,
	idDisciplina INT NOT NULL,
	idTurma      INT NOT NULL,
    idProfessor  INT NOT NULL,
    encerrada    BOOLEAN NOT NULL DEFAULT FALSE,
	coordenador  BOOLEAN NOT NULL DEFAULT FALSE,
    
    CONSTRAINT PK_dm PRIMARY KEY(id),
    CONSTRAINT FK_dm_disciplina FOREIGN KEY(idDisciplina) REFERENCES db_a9f943_pim.disciplina(id),
    CONSTRAINT FK_dm_turma      FOREIGN KEY(idTurma)      REFERENCES db_a9f943_pim.turma(id),
    CONSTRAINT FK_dm_professor  FOREIGN KEY(idProfessor)  REFERENCES db_a9f943_pim.professor(id)
) AUTO_INCREMENT = 10001;

CREATE TABLE IF NOT EXISTS db_a9f943_pim.conteudo (
	id                      INT          NOT NULL UNIQUE AUTO_INCREMENT,
    documentoURL            VARCHAR(500) NOT NULL,
    idDisciplinaMinistrada  INT          NOT NULL,

	CONSTRAINT PK_conteudo    PRIMARY KEY(id),
    CONSTRAINT FK_conteudo_dm FOREIGN KEY(idDisciplinaMinistrada) REFERENCES db_a9f943_pim.disciplinaMinistrada(id)
) AUTO_INCREMENT = 10001;

CREATE TABLE IF NOT EXISTS db_a9f943_pim.cursoMatriculado (
	id             INT     NOT NULL UNIQUE AUTO_INCREMENT,
    idAluno        INT     NOT NULL,
    idCurso        INT     NOT NULL,
    idTurma        INT     NOT NULL,
    semestreAtual  INT     NOT NULL DEFAULT 1,
    trancado       BOOLEAN NOT NULL DEFAULT FALSE,
    finalizado     BOOLEAN NOT NULL DEFAULT FALSE,
    
    CONSTRAINT PK_cm       PRIMARY KEY(id),
    CONSTRAINT FK_cm_aluno FOREIGN KEY(idAluno) REFERENCES db_a9f943_pim.aluno(id),
    CONSTRAINT FK_cm_curso FOREIGN KEY(idCurso) REFERENCES db_a9f943_pim.curso(id),
    CONSTRAINT FK_cm_turma FOREIGN KEY(idTurma) REFERENCES db_a9f943_pim.turma(id)
) AUTO_INCREMENT = 10001;

CREATE TABLE IF NOT EXISTS db_a9f943_pim.disciplinaCursada (
	id                 INT    NOT NULL UNIQUE AUTO_INCREMENT,
    idDisciplina       INT    NOT NULL,
    idCursoMatriculado INT    NOT NULL,
    prova1             DOUBLE,
    prova2             DOUBLE,
    trabalho           DOUBLE,
    media              DOUBLE,
    faltas             INT    NOT NULL DEFAULT 0,
    frequencia         DOUBLE,
    situacao           VARCHAR(10) NOT NULL DEFAULT "Cursando", # ("Cursando", "Aprovado", "Reprovado")   ,
	
    CONSTRAINT PK_dc            PRIMARY KEY(id),
    CONSTRAINT FK_dc_disciplina FOREIGN KEY(idDisciplina)       REFERENCES db_a9f943_pim.disciplina(id),
    CONSTRAINT FK_dc_cm         FOREIGN KEY(idCursoMatriculado) REFERENCES db_a9f943_pim.cursoMatriculado(id)
) AUTO_INCREMENT = 10001;

# n turma : (n professor : n disciplina)
CREATE TABLE IF NOT EXISTS db_a9f943_pim.disciplina_professor_turma (
    id           INT NOT NULL UNIQUE AUTO_INCREMENT,
	idDisciplina INT NOT NULL,
    idProfessor  INT NOT NULL,
    idTurma      INT NOT NULL,
    coordenador  BOOLEAN NOT NULL DEFAULT FALSE,
    
    CONSTRAINT PK_cpt            PRIMARY KEY(id),
    CONSTRAINT FK_cpt_disciplina FOREIGN KEY(idDisciplina) REFERENCES db_a9f943_pim.disciplina(id),
    CONSTRAINT FK_cpt_professor  FOREIGN KEY(idProfessor)  REFERENCES db_a9f943_pim.professor(id),
    CONSTRAINT FK_cpt_turma      FOREIGN KEY(idTurma)      REFERENCES db_a9f943_pim.turma(id)
) AUTO_INCREMENT = 10001;

# n curso : n disciplina
CREATE TABLE IF NOT EXISTS db_a9f943_pim.curso_disciplina (
	id                     INT NOT NULL UNIQUE AUTO_INCREMENT,
    idCurso                INT NOT NULL,
    idDisciplina INT NOT NULL,
    
    CONSTRAINT PK_cd            PRIMARY KEY(id),
    CONSTRAINT FK_cd_curso      FOREIGN KEY(idCurso) REFERENCES db_a9f943_pim.curso(id),
    CONSTRAINT FK_cd_disciplina FOREIGN KEY(idDisciplina) REFERENCES db_a9f943_pim.disciplina(id)
) AUTO_INCREMENT = 10001;

# 1 aluno : n cursoMatriculado
#CREATE TABLE IF NOT EXISTS db_a9f943_pim.aluno_cursoMatriculado ();

# 1 disciplinaCursada : n cursoMatriculado
#CREATE TABLE IF NOT EXISTS db_a9f943_pim.cursoMatriculado_disciplinaCursada ();

# 1 turma : n cursoMatriculado
#CREATE TABLE IF NOT EXISTS db_a9f943_pim.cursoMatriculado_turma ();

# 1 curso : n turma
#CREATE TABLE IF NOT EXISTS db_a9f943_pim.curso_turma ();

# 1 disciplina : n conteudo
#CREATE TABLE IF NOT EXISTS db_a9f943_pim.conteudo_disciplina ();

# 1 professor : n conteudo
#CREATE TABLE IF NOT EXISTS db_a9f943_pim.conteudo_professor ();

# 1 turma : n conteudo
#CREATE TABLE IF NOT EXISTS db_a9f943_pim.conteudo_turma ();