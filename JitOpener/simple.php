<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">
<html dir="ltr">
    
    <head>
        <meta http-equiv="Content-type" content="text/html; charset=utf-8" />
        <title>THE TITLE</title>
        <link rel="stylesheet" type="text/css" href="dijit/themes/claro/claro.css"
        />
        <link rel="stylesheet" type="text/css" href="dijit/themes/nihilo/nihilo.css"
        />
        <link rel="stylesheet" type="text/css" href="dijit/themes/soria/soria.css"
        />
        <style type="text/css">
            body, html { font-family:helvetica,arial,sans-serif; font-size:90%; }
            a { text-decoration:none; }
            a:hover { text-decoration:underline; }
        </style>
        <style type="text/css">
            html, body { width: 100%; height: 100%; margin: 0px; }
            #borderContainer { width: 100%; height: 100%; }
            
            .pretty-table th{background:#aaaabb; color:#fff;}
            .pretty-table td{
              background-color: #fff;
              color:#000;
              }
            
            .pretty-table { border-collapse: collapse; 
                            border: 1px solid #bbb; 
                            }
                            
            .pretty-table th, .pretty-table td { padding: 0.5em; 
                                                 border: 1px solid #bbb;
                                                 }
            .pretty-table tr  th[scope=row]
            { 
              background-color: #fff;
              color: #000;
            } 
                                                 
            .pretty-table td:hover 
            { 
              background-color: #ddd;
            } 


        </style>
        <link href="tablecloth/tablecloth.css" rel="stylesheet" type="text/css" media="screen" />
       
         <script type="text/javascript" src="tablecloth/tablecloth.js"></script>
         <script type="text/javascript">

             var _gaq = _gaq || [];
             _gaq.push(['_setAccount', 'UA-7604545-3']);
             _gaq.push(['_trackPageview']);

             (function () {
                 var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
                 ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
                 var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
             })();

</script>
    </head>
<body class=" claro ">
    <table width="100%" height="100%" 
    cellspacing=5px cellpadding=8px border=1px style="margin:0px; border-collapse:separate; border:1px #bbbbbb solid" >

    <tr><td colspan="2" height="30px" style="border:1px #bbbbbb solid">
        <a href="SIMPLE_PHP">SIMPLE_PAGE_TITLE</a> | <a href="blog/">The Blog</a> | <a href="INDEX_HTML">Cool Version</a> 
    </td></tr>

    <tr><td width="130px"  style="border:1px #bbbbbb solid; vertical-align:top">
          <!-- MENU -->
    </td>
    
    <td style="border:1px #bbbbbb solid; vertical-align:top" id="contentPane">
		
		<script type="text/javascript"><!--
		google_ad_client = "ca-pub-9952814099318731";
		/* AIKA PHP Site */
		google_ad_slot = "5151537152";
		google_ad_width = 468;
		google_ad_height = 15;
		//-->
		</script>
		<script type="text/javascript"
		src="//pagead2.googlesyndication.com/pagead/show_ads.js">
		</script>
		<br />

   <?php
   
        $str = $_SERVER['QUERY_STRING'];
        if (strlen($str) != 0) {
			$str = str_replace('-', '/', $str);
			$str = str_replace('con', 'ccon', $str);
			$str = trim($str, '-');
			
            $filename = 'data/'.$str.'.html';
			
            $handle = fopen($filename, "r");
            $content = fread($handle, filesize($filename));

            $content = preg_replace('/href="#/', 'href="SIMPLE_PHP?', $content);

            fclose($handle);
            echo $content;
        } else {

            echo "blog/news/stuff:<br />";
            // Include WordPress 
            define('WP_USE_THEMES', false);
            require('./blog/wp-load.php');
            query_posts('showposts=10');

            while (have_posts()): the_post(); 
            endwhile; 


            while (have_posts()): the_post(); ?>
            <h1><?php the_title(); ?></h1>
            <?php the_content(); ?>
            <p><a href="<?php the_permalink(); ?>">Read more...</a></p>
            <?php endwhile;
        }
    ?>
		
		<script type="text/javascript"><!--
		google_ad_client = "ca-pub-9952814099318731";
		/* AIKA PHP Site */
		google_ad_slot = "5151537152";
		google_ad_width = 468;
		google_ad_height = 15;
		//-->
		</script>
		<script type="text/javascript"
		src="//pagead2.googlesyndication.com/pagead/show_ads.js">
		</script>
		<br />
	
    </td></tr>

    <tr><td height="30px" colspan="2" style="border:1px #bbbbbb solid">
        Made by astrobunny. Images and descriptions by Hanbitsoft.
    </td></tr>
    </table>
    
</body>


</html>