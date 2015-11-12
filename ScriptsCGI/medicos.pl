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

my $Email = $cgi->param('Email');
my $Password = $cgi->param('Password');

exit 0 unless $Email && $Password; # This parameter is required

# Connect to a database
my $dbh = DBI->connect( 'DBI:mysql:bd_proyecto_cuerpo', 'dinorahcab', 'dinorahcab' )
    || die "Could not connect to database: $DBI::errstr";

# Insert the player score if there are any
#if( $IdExploracion && $IdVariable && $Valor) {
#$dbh->do( "insert into Mediciones (IdExploracion, IdVariable, Valor) values(?,?,?)",
#	  undef, $IdExploracion, $IdVariable, $Valor);
#}


# Medico registrado?
my $sth = $dbh->prepare(
    'SELECT IdMedico, Nombre, ApellidoP, ApellidoM FROM Medicos WHERE Email=? AND Password=?');
$sth->execute($Email, $Password);
while (my $r = $sth->fetchrow_arrayref) {
print join(':',@$r),"\n"
}
