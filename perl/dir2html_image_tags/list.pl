$count = 0;
if($ARGV[0] =~ /^$/ ){$dir = "";}
else{$dir = $ARGV[0] . "\\";}
if($ARGV[1] =~ /^(\d+)$/ ){$page = $1;}
else{$page = 25;}
$pre = "<IMG onclick=\"javascript:viewPic(this)\" width=60 src=\"";
$post = "\" border=1 /><br/>" . "\n";
while (<STDIN>){
  s/^\s.+$//;
  s/^\d\d-\d\d-\d\d\d\d  \d\d:\d\d\s+<DIR>\s.+$//;
  s/^\d\d-\d\d-\d\d\d\d  \d\d:\d\d\s+\d+\s//;
  s/\n$//;
  if($_ =~ /.+/){
    $count++;
    if($count % $page){
      print STDOUT $pre . $dir . $_ . $post;
    }else{
      print STDOUT $pre . $dir .  $_ . $post;
      print STDOUT "</TD><td vAlign=top>" . "\n";
    }
  }
}



