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
use CGI;
use DBI;

# Read form data etc.
my $cgi = new CGI;

# The results from the high score script will be in plain text
print $cgi->header("text/plain");

my $IdExploracion = $cgi->param('IdExploracion');
my $IdVariable = $cgi->param('IdVariable');
my $Valor = $cgi->param('Valor');

# exit 0 unless $IdExploracion && $IdVariable && $Valor; # This parameter is required

# Connect to a database
my $dbh = DBI->connect( 'DBI:mysql:bd_proyecto_cuerpo', 'dinorahcab', 'dinorahcab' )
    || die "Could not connect to database: $DBI::errstr";

# Insert the player score if there are any
if( $IdExploracion && $IdVariable && $Valor) {
$dbh->do( "insert into Mediciones (IdExploracion, IdVariable, Valor) values(?,?,?)",
	  undef, $IdExploracion, $IdVariable, $Valor);
}

# Fetch the high scores
my $sth = $dbh->prepare(
    'SELECT * FROM Mediciones WHERE IdExploracion=? AND IdVariable=? AND Valor=? ORDER BY Tiempo DESC' );
$sth->execute($IdExploracion, $IdVariable, $Valor);
while (my $r = $sth->fetchrow_arrayref) {
print join(':',@$r),"\n"
}
