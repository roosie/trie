//Roosevelt Young
//HW Trie CPTS 321 Evan Olds WSU
// Sept 19th 2016


//change log
//Jan 17th 2020 -- refactored trie class after feeling ashamed of the past
//              -- changed variable naming

//TODO
//Consider an iterative approach to finding all words 
//Implement delete a string
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace trie
{
    public partial class AutoCompleteUI : Form
    {
        trie init_trie;
        public AutoCompleteUI()
        {
            InitializeComponent();
            init_trie = new trie();
            //reads file form trie directory
            string[] readText = File.ReadAllLines("../../wordsEn.txt");

            //loads the source of the trie file into memory
            foreach(string word in readText){
                init_trie.insert(word);
            }
        }
        //if the textbox one is changed then it updates the listbox
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            listBox1.DataSource = init_trie.get_words(textBox1.Text);
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //out
        }
        class trie
        {
            Node root;
            //Node implementation
            class Node
            {
                public char value;
                public Node[] childern;
                public bool terminating;
                public Node parent;
                public Node(char new_char, Node n_parent)
                {
                    value = new_char;
                    childern = new Node[26];
                    terminating = false;
                    parent = n_parent;
                }
            }
            //sets base case of an empty node to be used as the head
            public trie()
            {
                root = new Node(' ', null);
            }

            //insert word by iterating through tree creating all missing nodes 
            public bool insert(string word)
            {
                Node curr = root;
                int char_to_int = 0;
                for (int i = 0; i < word.Length; i++)
                {
                    //get int value current char to use as a true hash in current's childern
                    char_to_int = (int)word[i] - 97;
                    if (char_to_int > -1 && char_to_int < 26)
                    {
                        if (curr.childern[char_to_int] == null)
                        {
                            curr.childern[char_to_int] = new Node(word[i], curr);
                        }
                        curr = curr.childern[char_to_int];
                    }
                }
                curr.terminating = true;
                return curr.terminating;
            }
            //iteratively goes through tree to find last char in word, recursively adds all terminating superstrings to word set
            public List<string> get_words(string word)
            {
                List<string> set = new List<string>();
                Node curr = root;
                int char_to_int = 0;
                for (int i = 0; i < word.Length; i++)
                {
                    //get int value current char to use as a true hash in current's childern
                    char_to_int = (int)word[i] - 97;
                    if (char_to_int > -1 && char_to_int < 26)
                    {
                        if (curr.childern[char_to_int] == null)
                        {
                            //returns an empty set if there are no matching strings or no substrings
                            return set;
                        }
                        curr = curr.childern[char_to_int];
                    }
                }
                add_all_childern(curr, word, set);
                return set;


            }
            //visits all remaining leaves in tree, if any node is a terminating node the string word is a inserted word and added to the tree
            private void add_all_childern(Node curr, string word, List<string> set)
            {
                if (curr == null)
                {
                    return;
                }
                if (curr.terminating)
                {
                    set.Add(word);
                }
                for (int i = 0; i < 26; i++)
                {
                    if (curr.childern[i] != null)
                    {
                        add_all_childern(curr.childern[i], word + curr.childern[i].value, set);
                    }
                }

            }
            //unused, deletes a node in the trie
            public bool delete(string word)
            {
                Node curr = root;
                int char_to_int = 0;
                //dfs into trie to see if word exists
                for (int i = 0; i < word.Length; i++)
                {
                    //get int value current char to use as a true hash in current's childern
                    char_to_int = (int)word[i] - 97;
                    if (curr.childern[char_to_int] == null)
                    {
                        return false;
                    }
                    curr = curr.childern[char_to_int];
                }
                //if word is terminatings, visits each parent and if parent isn't used to hold any other string memory is nulled
                if (curr.terminating)
                {
                    for (int i = 0; i < word.Length; i++)
                    {
                        for (int j = 0; j < 26; j++)
                        {
                            if (curr.childern[j] != null)
                            {
                                return true;
                            }
                        }
                        curr = curr.parent;
                        char_to_int = (int)word[word.Length - i - 1] - 97;
                        curr.childern[char_to_int] = null;
                    }
                }
                return false;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
