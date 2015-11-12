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

my $Identificacion = $cgi->param('Identificacion');
my $Nombre = $cgi->param('Nombre');
my $ApellidoP = $cgi->param('ApellidoP');
my $ApellidoM = $cgi->param('ApellidoM');
my $FechaNac = $cgi->param('FechaNac');

exit 0 unless $Identificacion || ($Nombre && $ApellidoP && $ApellidoM && $FechaNac); # This parameter is required

# Connect to a database
my $dbh = DBI->connect( 'DBI:mysql:bd_proyecto_cuerpo', 'dinorahcab', 'dinorahcab', {mysql_enable_utf8 => 1} )
    || die "Could not connect to database: $DBI::errstr";

# Insert the player score if there are any
#if( $IdExploracion && $IdVariable && $Valor) {
#$dbh->do( "insert into Pacientes (Identificacion, ApellidoP, ApellidoM, Nombre, FechaNac, Direccion, Telefono, Email, NoParejas, IdArea, Sexo, IVSA, IdMPF) values(?,?,?,?,?,?,?,?,?,?,?,?,?)",
#	  undef, $Identificacion, $ApellidoP, $ApellidoM, $Nombre, $FechaNac, $Direccion, $Telefono, $Email, $NoParejas, $IdArea, $Sexo, $IVSA, $IdMPF);
#}

# Paciente registrado?
if ($Identificacion) {
    my $sth = $dbh->prepare(
    'SELECT * FROM Pacientes WHERE Identificacion=?');
    $sth->execute($Identificacion);
    while (my $r = $sth->fetchrow_arrayref) {
	print join(':',@$r),"\n"
    }
} else {
    my $sth = $dbh->prepare(
    'SELECT * FROM Pacientes WHERE ApellidoP=? AND ApellidoM=? AND Nombre=? AND FechaNac=?');
    $sth->execute($ApellidoP, $ApellidoM, $Nombre, $FechaNac);
    while (my $r = $sth->fetchrow_arrayref) {
	print join(':',@$r),"\n"
    }
}

