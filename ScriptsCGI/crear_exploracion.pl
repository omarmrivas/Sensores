#!/usr/bin/perl
# The SQL database needs to have a table called highscores
# that looks something like this:
#
#   CREATE TABLE highscores (
#     game varchar(255) NOT NULL,
#     player varchar(255) NOT NULL,
#     score integer NOT NULL
#   );
#
use strict;
use CGI ( "-utf8" );
use DBI;
use utf8;

# Read form data etc.
my $cgi = new CGI;

# The results from the high score script will be in plain text
print $cgi->header("text/plain");

my $IdPaciente = $cgi->param('PacienteIdentificacion');
my $IdMedico = $cgi->param('MedicoId');

# Esta prueba falla si la variable contiene un "0"...
#exit 0 unless $IdMedico; # This parameter is required

# Connect to a database
my $dbh = DBI->connect( 'DBI:mysql:bd_proyecto_cuerpo', 'dinorahcab', 'dinorahcab', {mysql_enable_utf8 => 1} )
    || die "Could not connect to database: $DBI::errstr";

# Insert the player score if there are any
#if( $IdExploracion && $IdVariable && $Valor) {
$dbh->do( "insert into Exploracion_Fisica (IdPaciente, IdMedico) values(?,?)",
	  undef, $IdPaciente, $IdMedico);
#}

# Paciente registrado?
my $sth = $dbh->prepare(
    'SELECT * FROM Exploracion_Fisica WHERE IdPaciente=? AND IdMedico=? ORDER BY FechaEF DESC');
$sth->execute($IdPaciente, $IdMedico);
while (my $r = $sth->fetchrow_arrayref) {
print join(':',@$r),"\n"
}
